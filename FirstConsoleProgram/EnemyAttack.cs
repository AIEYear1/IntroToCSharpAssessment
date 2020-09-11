using System;
using System.Collections.Generic;
using System.Numerics;
using static RaylibWindowNamespace.Objects;
using static Raylib_cs.Color;
using static Raylib_cs.Raylib;
using CRPGNamespace;

namespace RaylibWindowNamespace
{
    public enum EnemyAttackIndex { WOLFATTACK, LOOTERATTACK, MERMAIDATTACK, TROLLATTACK }
    public class EnemyAttack
    {
        public int minDamage;
        public int maxDamage;
        public string description;
        readonly EnemyAttackIndex index;

        bool initialized = false;

        public EnemyAttack(int minDamage, int maxDamage, string description, EnemyAttackIndex index)
        {
            this.minDamage = minDamage;
            this.maxDamage = maxDamage;
            this.description = description;
            this.index = index;
        }

        public void Start()
        {
            initialized = false;
        }

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
            }
        }

        #region Wolf Attack
        List<AI> wolves;
        void WolfAttack()
        {
            //put player in bottom left corner have him try to catch enemy on collision enemy takes damage

            if (!initialized)
                InitWolf();

            player.Update();
            player.Draw();

            for (int x = 0; x < MathF.Min(wolves.Count * Window.attackTimer.PercentComplete * 2, wolves.Count); x++)
            {
                wolves[x].SetDirection(player.position - wolves[x].position);
                wolves[x].Update();
                wolves[x].Draw();

                for (int y = 0; y < wolves.Count; y++)
                {
                    if (CollisionManager.Colliding(wolves[x], wolves[y]))
                    {
                        CollisionManager.Push(wolves[x], wolves[y]);
                    }
                }

                if (CollisionManager.Colliding(player, wolves[x]))
                {
                    player.creature.TakeDamage(Utils.NumberBetween(minDamage, maxDamage));
                    if(player.creature != null)
                        healthBar.Width = ((float)player.creature.currentHP / (float)player.creature.maximumHP) * healthBackground.Width;
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
            wolves = new List<AI>();
            for (int x = 0; x < 5; x++) 
            {
                wolves.Add(new AI(monster.texture,
                                  new Vector2(Utils.NumberBetween((int)(Window.playZoneBarrier.X + 10), (int)(Window.playZoneBarrier.Z - 10)), 
                                  Utils.NumberBetween((int)(Window.playZoneBarrier.Y + 10), (int)(Window.playZoneBarrier.W - 10))),
                                  WHITE, 16, Vector2.One * 4, 10));

                wolves[x].speed = 350;
                wolves[x].sensitivity = 1.5f;
            }

            player.position = new Vector2(Window.screenWidth / 2, Window.screenHeight / 2);
            player.speed = 300;
            player.sensitivity = 3;

            initialized = true;
        }
        #endregion

        #region Looter Attack
        void LooterAttack()
        {
            //one looter spawned randomly in the scene, player cannot see the looter when it is behind them
            if (!initialized)
                InitLooter();

            player.Update();

            monster.SetDirection(player.position - monster.position);
            monster.Update();

            Vector2 triPoint2 = player.position + Utils.RotationMatrix(Utils.LockMagnitude(player.direction, 1), Utils.DegToRad(-70), Window.screenWidth / MathF.Cos(Utils.DegToRad(-70)));
            Vector2 triPoint3 = player.position + Utils.RotationMatrix(Utils.LockMagnitude(player.direction, 1), Utils.DegToRad(70), Window.screenWidth / MathF.Cos(Utils.DegToRad(70)));
            DrawTriangle(player.position, triPoint2, triPoint3, GRAY);

            player.Draw();

            if (MathF.Abs(Utils.AngleBetween(Utils.LockMagnitude(player.direction, 1), monster.position - player.position)) < 70) 
            {
                monster.Draw();
            }

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
            monster.position = new Vector2(monster.radius, Window.screenHeight - monster.radius);
            monster.sensitivity = 3;
            monster.speed = 200;

            player.position = new Vector2(Window.screenWidth / 2, Window.screenHeight / 2);
            player.sensitivity = 4;
            player.speed = 400;

            initialized = true;
        }
        #endregion

        #region Mermaid Attack
        List<LineSprite> spears = new List<LineSprite>();
        void MermaidAttack()
        {
            //spears appear pointing at the player and travel forward on collision player takes damage
            if (!initialized)
                InitMermaid();

            player.Update();
            player.Draw();

            for (int x = 0; x < MathF.Min((spears.Count + 1) * Window.attackTimer.PercentComplete, spears.Count); x++)
            {
                spears[x].Spawn(player.position);
                spears[x].Update();
                spears[x].Draw();

                if (CollisionManager.Colliding(player, spears[x]))
                {
                    player.creature.TakeDamage(Utils.NumberBetween(minDamage, maxDamage));
                    if (player.creature != null)
                        healthBar.Width = ((float)player.creature.currentHP / (float)player.creature.maximumHP) * healthBackground.Width;
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
            spears = new List<LineSprite>();
            for (int x = 0; x < 15; x++)
            {
                spears.Add(new LineSprite(Vector2.Zero, Vector2.Zero, 100, 10, 600, SKYBLUE));
            }

            player.position = new Vector2(Window.screenWidth / 2, Window.screenHeight / 2);
            player.sensitivity = 4;
            player.speed = 400;

            initialized = true;
        }
        #endregion
        List<RectangleSprite> attackSpaces = new List<RectangleSprite>();
        Timer waitTimer = new Timer(3);
        Timer stallTimer = new Timer(1.5f);
        Timer holdTimer = new Timer(.5f);
        int spaceToUse = -1;
        int prevSpace = -1;
        bool hit = false;
        void TrollAttack()
        {
            //troll will randomly attack the left right or center all of which take up have the screen player must avoid or take damage
            if (!initialized)
                InitTroll();

            player.Update();
            player.Draw();

            if (!waitTimer.Check(false))
                return;

            if (stallTimer.timer == 0)
            {
                spaceToUse = Utils.NumberBetween(0, attackSpaces.Count - 1);
                while(spaceToUse == prevSpace)
                {
                    spaceToUse = Utils.NumberBetween(0, attackSpaces.Count - 1);
                }
            }

            attackSpaces[spaceToUse].Draw();
            player.Draw();

            if (!stallTimer.Check(false))
            {
                attackSpaces[spaceToUse].color = GRAY;
                return;
            }

            if (!holdTimer.Check())
            {
                attackSpaces[spaceToUse].color = RED;

                if (!hit && CollisionManager.Colliding(player, attackSpaces[spaceToUse].Rectangle))
                {
                    player.creature.TakeDamage(Utils.NumberBetween(minDamage, maxDamage));
                    if (player.creature != null)
                        healthBar.Width = ((float)player.creature.currentHP / (float)player.creature.maximumHP) * healthBackground.Width;

                    hit = true;
                }

                return;
            }

            prevSpace = spaceToUse;
            stallTimer.Reset();
            waitTimer.Reset();
            attackSpaces[spaceToUse].color = BLANK;
            hit = false;
        }
        void InitTroll()
        {
            attackSpaces = new List<RectangleSprite>
            {
                new RectangleSprite(Vector2.Zero, Window.screenWidth / 2, Window.screenHeight, BLANK),
                new RectangleSprite(Vector2.UnitX * (Window.screenWidth / 4), Window.screenWidth / 2, Window.screenHeight, BLANK),
                new RectangleSprite(Vector2.UnitX * (Window.screenWidth / 2), Window.screenWidth / 2, Window.screenHeight, BLANK)
            };

            player.position = new Vector2(Window.screenWidth / 2, Window.screenHeight / 2);
            player.sensitivity = 4;
            player.speed = 400;

            initialized = true;
        }
    }
}
