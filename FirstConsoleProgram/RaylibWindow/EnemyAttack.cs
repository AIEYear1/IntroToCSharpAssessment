using System;
using System.Collections.Generic;
using System.Numerics;
using static Raylib_cs.Color;
using static Raylib_cs.Raylib;
using static RaylibWindowNamespace.Objects;

namespace RaylibWindowNamespace
{
    /// <summary>
    /// Holds all possible enmy attacks
    /// </summary>
    public enum EnemyAttackIndex { WOLFATTACK, LOOTERATTACK, MERMAIDATTACK, TROLLATTACK, BANDITSATTACK }
    /// <summary>
    /// This class handles an enemy's attack
    /// </summary>
    public class EnemyAttack
    {
        /// <summary>
        /// The least damage the attack can do per hit
        /// </summary>
        public int minDamage;
        /// <summary>
        /// The most damage the attack can do per hit
        /// </summary>
        public int maxDamage;
        /// <summary>
        /// Attack description
        /// </summary>
        public string description;
        /// <summary>
        /// The attack type this attack is
        /// </summary>
        readonly EnemyAttackIndex index;

        public EnemyAttack(int minDamage, int maxDamage, string description, EnemyAttackIndex index)
        {
            this.minDamage = minDamage;
            this.maxDamage = maxDamage;
            this.description = description;
            this.index = index;
        }

        /// <summary>
        /// Initializes the main attack
        /// </summary>
        public void Start()
        {
            switch (index)
            {
                case EnemyAttackIndex.WOLFATTACK:
                    InitWolf();
                    break;
                case EnemyAttackIndex.LOOTERATTACK:
                    InitLooter();
                    break;
                case EnemyAttackIndex.MERMAIDATTACK:
                    InitMermaid();
                    break;
                case EnemyAttackIndex.TROLLATTACK:
                    InitTroll();
                    break;
                case EnemyAttackIndex.BANDITSATTACK:
                    InitBandits();
                    break;
            }
        }

        /// <summary>
        /// Updates the main atttack
        /// </summary>
        public void Update()
        {
            switch (index)
            {
                case EnemyAttackIndex.WOLFATTACK:
                    WolfAttack();
                    break;
                case EnemyAttackIndex.LOOTERATTACK:
                    LooterAttack();
                    break;
                case EnemyAttackIndex.MERMAIDATTACK:
                    MermaidAttack();
                    break;
                case EnemyAttackIndex.TROLLATTACK:
                    TrollAttack();
                    break;
                case EnemyAttackIndex.BANDITSATTACK:
                    BanditsAttack();
                    break;
            }
        }

        #region Wolf Attack
        readonly List<AI> wolves = new List<AI>();
        void WolfAttack()
        {
            //Wolves spawn randomly throughout the scene over time and if a wolf collides with player, player takes damage

            //Update the player
            player.Update();
            player.Draw();

            //Update Wolves
            for (int x = 0; x < MathF.Min(wolves.Count * Window.attackTimer.PercentComplete * 2, wolves.Count); x++)
            {
                wolves[x].SetDirection(player.Position - wolves[x].Position);
                wolves[x].Update();
                wolves[x].Draw();

                //make sure the wolves never overlap
                for (int y = 0; y < wolves.Count; y++)
                {
                    if (CollisionManager.Colliding(wolves[x], wolves[y]))
                    {
                        CollisionManager.Push(wolves[x], wolves[y]);
                    }
                }

                //Check to see if a wolf hit the player
                if (CollisionManager.Colliding(player, wolves[x]))
                {
                    int damage = Utils.NumberBetween(minDamage, maxDamage);

                    player.creature.TakeDamage(damage);

                    if (player.creature != null)
                    {
                        damage -= (player.creature as CRPGNamespace.Player).CurrentAc;
                        damage = (int)MathF.Max(damage, 1);
                        player.PopUp(damage.ToString(), (int)Utils.Lerp(10, 70, damage / player.creature.maximumHP));
                        healthBar.Width = ((float)player.creature.currentHP / (float)player.creature.maximumHP) * healthBackground.Width;
                    }

                    wolves.RemoveAt(x);

                    if (wolves.Count == 0)
                    {
                        Window.attackTimer.Reset(Window.attackTimer.delay);
                    }
                }
            }
        }
        void InitWolf()
        {
            wolves.Clear();
            //Initialize the wolves
            for (int x = 0; x < 5; x++)
            {
                wolves.Add(new AI(monster.texture,
                                  new Vector2(Utils.NumberBetween((int)(Window.playZoneBarrier.X + 10), (int)(Window.playZoneBarrier.Z - 10)),
                                  Utils.NumberBetween((int)(Window.playZoneBarrier.Y + 10), (int)(Window.playZoneBarrier.W - 10))),
                                  WHITE, 16, Vector2.One * 4, 10));

                wolves[x].speed = 350;
                wolves[x].sensitivity = 1.5f;
            }

            //Initialize the player
            player.Position = new Vector2(Window.screenWidth / 2, Window.screenHeight / 2);
            player.speed = 300;
            player.sensitivity = 3;
        }
        #endregion

        #region Looter Attack
        void LooterAttack()
        {
            //one looter spawned in the bottom left corner of the scene, player cannot see the looter when it is behind them, on collision player takes damage
            #region Create and update player view triangle
            Vector2 triPoint2 = player.Position + Utils.RotationMatrix(Utils.LockMagnitude(player.direction, 1), Utils.DegToRad(-70), Window.screenWidth / MathF.Cos(Utils.DegToRad(-70)));
            Vector2 triPoint3 = player.Position + Utils.RotationMatrix(Utils.LockMagnitude(player.direction, 1), Utils.DegToRad(70), Window.screenWidth / MathF.Cos(Utils.DegToRad(70)));
            DrawTriangle(player.Position, triPoint2, triPoint3, GRAY);
            #endregion

            player.Update();
            player.Draw();

            monster.SetDirection(player.Position - monster.Position);
            monster.Update();

            //If monster is within view of player draw it
            if (MathF.Abs(Utils.AngleBetween(Utils.LockMagnitude(player.direction, 1), monster.Position - player.Position)) < 70)
            {
                monster.Draw();
                monster.speed = 300;
            }
            else
            {
                monster.speed = 100;
            }

            //check to see if monster has collided with the player
            if (CollisionManager.Colliding(player, monster))
            {
                player.creature.TakeDamage(Utils.NumberBetween(minDamage, maxDamage));
                if (player.creature != null)
                    healthBar.Width = ((float)player.creature.currentHP / (float)player.creature.maximumHP) * healthBackground.Width;
                Window.attackTimer.Reset(Window.attackTimer.delay);
            }
        }
        void InitLooter()
        {
            //Initialize monster
            monster.Position = new Vector2(Window.playZoneBarrier.X + monster.radius, Window.playZoneBarrier.W - monster.radius);
            monster.sensitivity = 3f;
            monster.speed = 100;

            //Initialize player
            player.Position = new Vector2(Window.screenWidth / 2, Window.screenHeight / 2);
            player.sensitivity = 4;
            player.speed = 400;
        }
        #endregion

        #region Mermaid Attack
        readonly List<LineSprite> spears = new List<LineSprite>();
        void MermaidAttack()
        {
            //spears appear pointing at the player and travel forward on collision player takes damage

            //Update the player
            player.Update();
            player.Draw();

            //Update spears
            for (int x = 0; x < MathF.Min((spears.Count + 1) * Window.attackTimer.PercentComplete, spears.Count); x++)
            {
                spears[x].Spawn(player.Position);
                spears[x].Update();
                spears[x].Draw();

                //Check for collision
                if (CollisionManager.Colliding(player, spears[x]))
                {
                    int damage = Utils.NumberBetween(minDamage, maxDamage);

                    player.creature.TakeDamage(damage);

                    if (player.creature != null)
                    {
                        damage -= (player.creature as CRPGNamespace.Player).CurrentAc;
                        damage = (int)MathF.Max(damage, 1);
                        player.PopUp(damage.ToString(), (int)Utils.Lerp(10, 70, damage / player.creature.maximumHP));
                        healthBar.Width = ((float)player.creature.currentHP / (float)player.creature.maximumHP) * healthBackground.Width;
                    }

                    spears.RemoveAt(x);

                    if (spears.Count == 0)
                    {
                        Window.attackTimer.Reset(Window.attackTimer.delay);
                    }
                }
            }
        }
        void InitMermaid()
        {
            spears.Clear();
            //Initialize the spears
            for (int x = 0; x < 15; x++)
            {
                spears.Add(new LineSprite(Vector2.Zero, Vector2.Zero, 100, 10, 600, SKYBLUE));
            }

            //Initialize the player
            player.Position = new Vector2(Window.screenWidth / 2, Window.screenHeight / 2);
            player.sensitivity = 4;
            player.speed = 400;
        }
        #endregion

        #region Troll Attack
        List<RectangleSprite> attackSpaces;
        /// <summary>
        /// waits to give the player some pause between attacks
        /// </summary>
        Timer waitTimer = new Timer(0.5f);
        /// <summary>
        /// stalls so the player can react to the incoming attack
        /// </summary>
        Timer stallTimer = new Timer(1.1f);
        /// <summary>
        /// holds so the player can see the attack
        /// </summary>
        Timer holdTimer = new Timer(0.3f);
        /// <summary>
        /// attack space to use
        /// </summary>
        int spaceToUse = -1;
        /// <summary>
        /// previous attack space used to limit repeats
        /// </summary>
        int prevSpace = -1;
        /// <summary>
        /// keeps the player from being hit multiple times by the same attack
        /// </summary>
        bool hit = false;
        void TrollAttack()
        {
            //troll will randomly attack the left right or center all of which take up have the screen player must avoid or take damage

            //Update the player
            player.Update();
            player.Draw();

            //If it hasn't been long enough inbetween attack return
            if (!waitTimer.Check(false))
                return;

            //for the first frame decide which space to attack from
            if (stallTimer.Time == 0)
            {
                spaceToUse = Utils.NumberBetween(0, attackSpaces.Count - 1);
                while (spaceToUse == prevSpace)
                {
                    spaceToUse = Utils.NumberBetween(0, attackSpaces.Count - 1);
                }
            }

            attackSpaces[spaceToUse].Draw();
            player.Draw();//Make sure to draw the player over the space

            //if it hasn't been long enough to give the player a chance to react set color to grey and return
            if (!stallTimer.Check(false))
            {
                attackSpaces[spaceToUse].color = GRAY;
                return;
            }

            //hold the damage zone so the player can see it
            if (!holdTimer.Check())
            {
                attackSpaces[spaceToUse].color = RED;

                if (!hit && CollisionManager.Colliding(player, attackSpaces[spaceToUse].Rectangle))
                {
                    int damage = Utils.NumberBetween(minDamage, maxDamage);

                    player.creature.TakeDamage(damage);

                    if (player.creature != null)
                    {
                        damage -= (player.creature as CRPGNamespace.Player).CurrentAc;
                        damage = (int)MathF.Max(damage, 1);
                        player.PopUp(damage.ToString(), (int)Utils.Lerp(10, 70, damage / player.creature.maximumHP));
                        healthBar.Width = ((float)player.creature.currentHP / (float)player.creature.maximumHP) * healthBackground.Width;
                    }

                    hit = true;
                }

                return;
            }

            //reset and run again
            prevSpace = spaceToUse;
            stallTimer.Reset();
            waitTimer.Reset();
            attackSpaces[spaceToUse].color = BLANK;
            hit = false;
        }
        void InitTroll()
        {
            //Initialize attack spaces
            attackSpaces = new List<RectangleSprite>
            {
                new RectangleSprite(Vector2.Zero, Window.screenWidth / 2, Window.screenHeight, BLANK),
                new RectangleSprite(Vector2.UnitX * (Window.screenWidth / 4), Window.screenWidth / 2, Window.screenHeight, BLANK),
                new RectangleSprite(Vector2.UnitX * (Window.screenWidth / 2), Window.screenWidth / 2, Window.screenHeight, BLANK)
            };

            //Initialize the player
            player.Position = new Vector2(Window.screenWidth / 2, Window.screenHeight / 2);
            player.sensitivity = 4;
            player.speed = 400;

            //Reset the timers
            holdTimer.Reset();
            stallTimer.Reset();
            waitTimer.Reset();
        }
        #endregion

        #region Bandits Attack
        //Second monster melee
        AI monster2;
        Timer rangedTimer = new Timer(2);
        readonly List<LineSprite> arrows = new List<LineSprite>();
        Timer playerInvulnerability = new Timer(0.15f);
        int state = 0;
        void BanditsAttack()
        {
            //Have two bandits attack the player one using melee and one range

            //Updates
            player.Update();
            player.Draw();

            Vector2 dir = player.Position - monster.Position;
            if (MathF.Abs(dir.X) > MathF.Abs(dir.Y)) 
            {
                state = (dir.X > 0) ? 2 : 1;
            }
            else if (MathF.Abs(dir.X) < MathF.Abs(dir.Y))
            {
                state = (dir.Y > 0) ? 0 : 3;
            }
            monster.SetState(state);
            monster.Draw();

            monster2.SetDirection(player.Position - monster2.Position);
            monster2.Update();
            monster2.Draw();
            //make monster2 collide with monster
            if (CollisionManager.Colliding(monster, monster2))
            {
                CollisionManager.Push(monster, monster2);
            }

            //have monster one shoot a new arrow
            if (rangedTimer.Check())
            {
                arrows.Add(new LineSprite(monster.Position, player.Position - monster.Position, 60, 10, 600, BROWN));
            }
            //Update arrows
            for (int x = 0; x < arrows.Count; x++)
            {
                arrows[x].Update();
                arrows[x].Draw();

                //Check to see if arrow has collided with player
                if(CollisionManager.Colliding(player, arrows[x]))
                {
                    int damage = Utils.NumberBetween(minDamage, maxDamage);

                    player.creature.TakeDamage(damage);

                    if (player.creature != null)
                    {
                        damage -= (player.creature as CRPGNamespace.Player).CurrentAc;
                        damage = (int)MathF.Max(damage, 1);
                        player.PopUp(damage.ToString(), (int)Utils.Lerp(10, 70, damage / player.creature.maximumHP));
                        healthBar.Width = ((float)player.creature.currentHP / (float)player.creature.maximumHP) * healthBackground.Width;
                    }

                    arrows.RemoveAt(x);
                }
            }

            //ensure the player doesn't get sawbladed by the enemies
            if (playerInvulnerability.Check(false))
            {
                //check to see if monster has collided with the player
                if (CollisionManager.Colliding(player, monster))
                {
                    player.creature.TakeDamage(Utils.NumberBetween(minDamage, maxDamage));
                    if (player.creature != null)
                        healthBar.Width = ((float)player.creature.currentHP / (float)player.creature.maximumHP) * healthBackground.Width;

                    CollisionManager.Push(monster, player);
                    playerInvulnerability.Reset();
                }
                //check to see if monster2 has collided with the player
                if (CollisionManager.Colliding(player, monster2))
                {
                    player.creature.TakeDamage(Utils.NumberBetween(minDamage, maxDamage));
                    if (player.creature != null)
                        healthBar.Width = ((float)player.creature.currentHP / (float)player.creature.maximumHP) * healthBackground.Width;

                    CollisionManager.Push(monster2, player);
                    playerInvulnerability.Reset();

                    monster2.Position = monster.Position;
                }
            }
        }

        void InitBandits()
        {
            arrows.Clear();
            monster.Position = new Vector2(Utils.NumberBetween((int)(Window.playZoneBarrier.X + 10), (int)(Window.playZoneBarrier.Z - 10)),
                                  Utils.NumberBetween((int)(Window.playZoneBarrier.Y + 10), (int)(Window.playZoneBarrier.W - 10)));
            monster.speed = 0;
            monster.sensitivity = 0;

            monster2 = new AI(monster.texture, monster.Position, WHITE, 16, Vector2.One * 4, monster.radius);
            CollisionManager.Push(monster, monster2);
            monster2.speed = 500;
            monster2.sensitivity = 1.5f;


            player.Position = new Vector2(Window.screenWidth / 2, Window.screenHeight / 2);
            player.sensitivity = 4;
            player.speed = 400;

            rangedTimer.Reset();
            playerInvulnerability.Reset();
        }
        #endregion
    }
}
