using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGThing
{
    public class Monster : LivingCreature
    {
        public Name name = null;
        public int maximumDamage = 0;
        public int minimumDamage = 0;
        public int rewardXP = 0;
        public int rewardGold = 0;

        public Monster(Name name, int maximumDamage, int minimumDamage, int rewardXP, int rewardGold, int HP) : base(HP)
        {
            this.name = name;
            this.maximumDamage = maximumDamage;
            this.minimumDamage = minimumDamage;
            this.rewardXP = rewardXP;
            this.rewardGold = rewardGold;
        }

        public void Attack(Player player)
        {
            int damage = RandomNumberGenerator.NumberBetween(minimumDamage, maximumDamage);
            player.currentHP -= damage;
            Utils.Add($"You were hit by {name.FullName} for {damage} damage!");

            if (player.currentHP <= 0)
            {
                Utils.Add(player.name.FullName + " has died!");
                Program.running = false;
            }
        }
    }
}
