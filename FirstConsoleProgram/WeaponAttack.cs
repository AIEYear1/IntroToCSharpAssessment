using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;
using static Raylib_cs.Color;
using static Raylib_cs.Raylib;
using static RaylibWindowNamespace.Objects;

namespace RaylibWindowNamespace
{
    public enum WeaponAttackIndex { STICKATTACK, MERMAIDSPEARATTACK }
    public class WeaponAttack
    {
        public int minDamage;
        public int maxDamage;
        public string description;

        WeaponAttackIndex index;

        bool initialized = false;

        public WeaponAttack(int minDamage, int maxDamage, string description, WeaponAttackIndex index)
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
            if (!initialized)
                InitStick();

            player.Update();

            Vector2 playerCourse = player.position + (Utils.LockMagnitude(player.direction, 1) * Vector2.Distance(monster.position, player.position));
            monster.SetDirection(monster.position - playerCourse);
            monster.Update();

            player.Draw();
            monster.Draw();

            if(CollisionManager.Colliding(player, monster))
            {
                monster.creature.TakeDamage(Utils.NumberBetween(minDamage, maxDamage));
                if(monster.creature != null)
                    healthBar.width = ((float)monster.creature.currentHP / (float)monster.creature.maximumHP) * healthBackground.width;
                Window.attackTimer.Reset(Window.attackTimer.delay);
            }
        }
        void InitStick()
        {
            player.position = new Vector2(player.radius, Window.screenHeight - player.radius);
            player.sensitivity = 2.5f;
            player.speed = 400;

            monster.position = new Vector2(Window.screenWidth / 2, Window.screenHeight / 2);
            monster.sensitivity = 3;
            monster.speed = 300;

            initialized = true;
        }
        #endregion

        #region Mermaid Spear Attack
        List<LineSprite> spears = new List<LineSprite>();
        Vector2 spawnPoint;
        int spearNum = 0;
        Timer endTimer = new Timer(2);
        void MermaidSpearAttack()
        {
            //spears appear pointing at the player and travel forward on collision player takes damage
            if (!initialized)
                InitMermaidSpear();

            if(spearNum < spears.Count)
            {
                if (IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    spawnPoint = GetMousePosition();
                }
                if (IsMouseButtonReleased(MouseButton.MOUSE_LEFT_BUTTON))
                {
                    spears[spearNum].position = spawnPoint;
                    spears[spearNum].Spawn(GetMousePosition(), true);
                    spearNum++;
                }
            }
            else if (endTimer.Check())
            {
                Window.attackTimer.Reset(Window.attackTimer.delay);
            }
            
            
            if(spearNum == 0)
            {
                monster.SetDirection(monster.position - GetMousePosition());
                monster.Update();
                monster.Draw();
                return;
            }

            float distance = Vector2.Distance(monster.position, spears[0].position);
            int spearToRunFrom = 0;
            for (int x = 0; x < spearNum; x++) 
            {
                if(x >= spears.Count)
                {
                    break;
                }
                spears[x].Update();
                spears[x].Draw();

                if (CollisionManager.Colliding(monster, spears[x]))
                {
                    monster.creature.TakeDamage(Utils.NumberBetween(minDamage, maxDamage));
                    if (monster.creature != null)
                        healthBar.width = ((float)monster.creature.currentHP / (float)monster.creature.maximumHP) * healthBackground.width;
                    spears.RemoveAt(x);
                    if (spears.Count == 0)
                    {
                        Window.attackTimer.Reset(Window.attackTimer.delay);
                    }
                    continue;
                }

                if (distance > Vector2.Distance(monster.position, spears[x].position))
                {
                    spearToRunFrom = x;
                    distance = Vector2.Distance(monster.position, spears[x].position);
                }
            }

            monster.SetDirection(monster.position - spears[spearToRunFrom].position);
            monster.Update();
            monster.Draw();
        }
        void InitMermaidSpear()
        {
            spears = new List<LineSprite>();
            for (int x = 0; x < 15; x++)
            {
                spears.Add(new LineSprite(Vector2.Zero, Vector2.Zero, 100, 10, 600, SKYBLUE));
            }

            monster.position = new Vector2(Window.screenWidth / 2, Window.screenHeight / 2);
            monster.sensitivity = 4;
            monster.speed = 400;

            initialized = true;
        }
        #endregion
    }
}
