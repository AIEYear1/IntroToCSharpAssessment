using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace FirstConsoleProgram
{
    public class Player : LivingCreature
    {
        public Name name;
        public int gold = 0;
        public int XP = 0;
        public int level = 1;
        public Weapon currentWeapon;
        public Armor currentArmor;

        public Player(Name name, int gold, int xP, int level, Weapon currentWeapon, Armor currentArmor, int HP) : base(HP)
        {
            this.name = name;
            this.gold = gold;
            XP = xP;
            this.level = level;
            this.currentWeapon = currentWeapon;
            this.currentArmor = currentArmor;
        }

        public void Attack(Monster enemToAttack)
        {
            if (currentWeapon == null)
            {
                Console.WriteLine("You need a weapon to attack");
                return;
            }
            if (enemToAttack == null)
            {
                Console.WriteLine("You swing wildly at the air");
                return;
            }

            int damage = RandomNumberGenerator.NumberBetween(currentWeapon.minDamage, currentWeapon.maxDamage);
            enemToAttack.currentHP -= damage;
            Console.WriteLine($"You hit {enemToAttack.name.FullName} for {damage} damage!");

            if (enemToAttack.currentHP <= 0)
            {
                Console.WriteLine(enemToAttack.name.FullName + " has died");
                gold += enemToAttack.rewardGold;
                XP += enemToAttack.rewardXP;
                return;
            }

            enemToAttack.Attack(this);
        }
    }
}
