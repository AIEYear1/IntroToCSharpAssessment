using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;
using static Raylib_cs.Color;
using static Raylib_cs.Raylib;
using static RaylibWindowNamespace.Objects;

namespace RaylibWindowNamespace
{
    /// <summary>
    /// Holds all possible weapon attacks
    /// </summary>
    public enum WeaponAttackIndex { STICKATTACK, MERMAIDSPEARATTACK }
    /// <summary>
    /// This class handles a weapons's attack
    /// </summary>
    public class WeaponAttack
    {
        /// <summary>
        /// Minimum damage this attack can do per hit
        /// </summary>
        public int minDamage;
        /// <summary>
        /// Maximum damage this attack can do per hit
        /// </summary>
        public int maxDamage;
        /// <summary>
        /// Description of the attack
        /// </summary>
        public string description;
        /// <summary>
        /// What attack type this attack is
        /// </summary>
        readonly WeaponAttackIndex index;

        /// Paramters
        /// <param name="minDamage"></param>
        /// <param name="maxDamage"></param>
        /// <param name="description"></param>
        /// <param name="index"></param>
        public WeaponAttack(int minDamage, int maxDamage, string description, WeaponAttackIndex index)
        {
            this.minDamage = minDamage;
            this.maxDamage = maxDamage;
            this.description = description;
            this.index = index;
        }

        /// <summary>
        /// Initializes the weapon attack
        /// </summary>
        public void Start()
        {
            switch (index)
            {
                case WeaponAttackIndex.STICKATTACK:
                    InitStick();
                    break;
                case WeaponAttackIndex.MERMAIDSPEARATTACK:
                    InitMermaidSpear();
                    break;
            }
        }

        /// <summary>
        /// Updates the weapon attack
        /// </summary>
        public void Update()
        {
            switch (index)
            {
                case WeaponAttackIndex.STICKATTACK:
                    StickAttack();
                    break;
                case WeaponAttackIndex.MERMAIDSPEARATTACK:
                    MermaidSpearAttack();
                    break;
            }
        }

        #region Stick Attack
        void StickAttack()
        {
            //put player in bottom left corner have him try to catch enemy on collision enemy takes damage

            player.Update();

            //Make the monster run away from the player and away from the direction the player is heading
            Vector2 playerCourse = player.Position + (Utils.LockMagnitude(player.direction, 1) * Vector2.Distance(monster.Position, player.Position));
            monster.SetDirection(monster.Position - playerCourse);
            monster.Update();

            player.Draw();
            monster.Draw();

            //Check for collision and if so damage the enemy and end the players turn
            if (CollisionManager.Colliding(player, monster))
            {
                monster.creature.TakeDamage((player.creature as CRPGNamespace.Player).Damage);
                if (monster.creature != null)
                    healthBar.Width = ((float)monster.creature.currentHP / (float)monster.creature.maximumHP) * healthBackground.Width;
                Window.attackTimer.Reset(Window.attackTimer.delay);
            }
        }
        void InitStick()
        {
            //Initialize the player
            player.Position = new Vector2(Window.playZoneBarrier.X + player.radius, Window.playZoneBarrier.W - player.radius);
            player.sensitivity = 2.5f;
            player.speed = 400;

            //Initialize the monster
            monster.Position = new Vector2(Window.screenWidth / 2, Window.screenHeight / 2);
            monster.sensitivity = 3;
            monster.speed = 300;
        }
        #endregion

        #region Mermaid Spear Attack
        readonly List<LineSprite> spears = new List<LineSprite>();
        Vector2 spawnPoint;
        Timer endTimer = new Timer(2);
        void MermaidSpearAttack()
        {
            //spears appear pointing at the player and travel forward on collision player takes damage

            //if the player hasn't used up all their spears and press and release the left mouse button spawn a new spear traveling in the direction the player dragged
            if (spears.Count < 15)
            {
                if (IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    spawnPoint = GetMousePosition();
                }
                if (IsMouseButtonReleased(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    spears.Add(new LineSprite(spawnPoint, GetMousePosition() - spawnPoint, 100, 10, 600, SKYBLUE));
                }
            }
            else if (endTimer.Check())
            {
                Window.attackTimer.Reset(Window.attackTimer.delay);
                return;
            }

            //If there are no active spears make the monster run from the cursor and return
            if (spears.Count == 0)
            {
                monster.SetDirection(monster.Position - GetMousePosition());
                monster.Update();
                monster.Draw();
                return;
            }

            float distance = Vector2.Distance(monster.Position, spears[0].position);
            int spearToRunFrom = 0;
            //Update spears
            for (int x = 0; x < spears.Count; x++)
            {
                spears[x].Update();
                spears[x].Draw();

                //Checks to see if monster was hit and if so, deal damage
                if (CollisionManager.Colliding(monster, spears[x]))
                {
                    int damage = (player.creature as CRPGNamespace.Player).Damage;

                    monster.creature.TakeDamage(damage);

                    if (monster.creature != null)
                    {
                        monster.PopUp(damage.ToString(), (int)Utils.Lerp(10, 70, damage / player.creature.maximumHP));
                        healthBar.Width = ((float)monster.creature.currentHP / (float)monster.creature.maximumHP) * healthBackground.Width;
                    }

                    spears.RemoveAt(x);

                    if (spears.Count != 0)
                        continue;

                    //if there are no spears run from mouse and don't continue
                    monster.SetDirection(monster.Position - GetMousePosition());
                    monster.Update();
                    monster.Draw();
                    return;
                }

                //Determine which spear to make the monster run from
                if (distance > Vector2.Distance(monster.Position, spears[x].position))
                {
                    spearToRunFrom = x;
                    distance = Vector2.Distance(monster.Position, spears[x].position);
                }
            }

            monster.SetDirection(monster.Position - spears[spearToRunFrom].position);
            monster.Update();
            monster.Draw();
        }
        void InitMermaidSpear()
        {
            spears.Clear();

            //Initialize monster
            monster.Position = new Vector2(Window.screenWidth / 2, Window.screenHeight / 2);
            monster.sensitivity = 4;
            monster.speed = 400;
        }
        #endregion
    }
}
