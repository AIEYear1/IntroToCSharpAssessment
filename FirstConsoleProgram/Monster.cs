using System;
using System.Collections.Generic;
using System.Text;

namespace FirstConsoleProgram
{
    public class Monster : LivingCreature
    {
        public Name name = null;
        public int maximumDamage = 0;
        public int minimumDamage = 0;
        public int rewardXP = 0;
        public int rewardGold = 0;

        public Monster(Name name, int maximumDamage, int minimumDamage, int rewardXP, int rewardGold, int currentHP, int maximumHP) : base(currentHP, maximumHP)
        {
            this.name = name;
            this.maximumDamage = maximumDamage;
            this.minimumDamage = minimumDamage;
            this.rewardXP = rewardXP;
            this.rewardGold = rewardGold;
        }

        public void Attack(Player player)
        {
            player.currentHP -= RandomNumberGenerator.NumberBetween(minimumDamage, maximumDamage);

            if (player.currentHP <= 0)
            {
                Console.WriteLine(player.name.FullName + " has died!");
                Program.running = false;
            }
        }
    }
}
