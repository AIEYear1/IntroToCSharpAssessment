using System;
using System.Collections.Generic;
using System.Numerics;
using static RaylibWindowNamespace.Objects;
using static Raylib_cs.Color;
using static Raylib_cs.Raylib;

namespace RaylibWindowNamespace
{
    public enum EnemyAttackIndex { WOLFATTACK, LOOTERATTACK, MERMAIDATTACK, TROLLATTACK }
    public class EnemyAttack
    {
        public int minDamage;
        public int maxDamage;
        public string description;

        EnemyAttackIndex index;

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
                        healthBar.width = ((float)player.creature.currentHP / (float)player.creature.maximumHP) * healthBackground.width;
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

            player.speed = 300;
            player.sensitivity = 3;
            initialized = true;
        }
        #endregion

        #region Looter Attack
        //TODO: continue testing anglebetween code, I don't think its doing what I want it to do
        void LooterAttack()
        {
            //one looter spawned randomly in the scene, player cannot see the looter when it is behind them

            if (!initialized)
                InitLooter();

            player.Update();

            monster.SetDirection(player.position - monster.position);
            monster.Update();

            player.Draw();

            if (MathF.Abs(Utils.AngleBetween(player.position + Utils.LockMagnitude(player.direction, 4), monster.position - player.position)) < 70) 
            {
                monster.Draw();
            }

            if (CollisionManager.Colliding(player, monster))
            {
                player.creature.TakeDamage(Utils.NumberBetween(minDamage, maxDamage));
                if (player.creature != null)
                    healthBar.width = ((float)player.creature.currentHP / (float)player.creature.maximumHP) * healthBackground.width;
                Window.attackTimer.Reset(Window.attackTimer.delay);
            }
        }
        void InitLooter()
        {
            monster.position = new Vector2(monster.radius, Window.screenHeight - monster.radius);
            monster.sensitivity = 3;
            monster.speed = 300;

            player.sensitivity = 4;
            player.speed = 400;

            initialized = true;
        }
        #endregion
        void MermaidAttack()
        {
            //spears appear pointing at the player and travel forward on collision player takes damage
        }
        void TrollAttack()
        {
            //troll will randomly attack the left right or center all of which take up have the screen player must avoid or take damage
        }
    }
}
