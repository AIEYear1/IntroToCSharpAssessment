using CRPGNamespace;
using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;
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
                healthBar.width = ((float)monster.creature.currentHP / (float)monster.creature.maximumHP) * healthBackground.width;
                Window.attackTimer.Reset(Window.attackTimer.delay);
            }
        }
        void InitStick()
        {
            player.position = new Vector2(player.radius, Window.screenHeight - player.radius);
            player.sensitivity = 2;
            monster.sensitivity = 2.5f;
            player.speed = 400;
            monster.speed = 300;
            initialized = true;
        }

        void MermaidSpearAttack()
        {
            //player clicks and drags in the direction they want to attack spear spawns on release on collision enemy takes damage
        }
    }
}
