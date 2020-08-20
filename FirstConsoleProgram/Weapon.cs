using System;
using System.Collections.Generic;
using System.Text;

namespace FirstConsoleProgram
{
    public class Weapon : Item
    {
        public int maxDamage = 0;
        public int minDamage = 0;

        public Weapon(int maxDamage, int minDamage, string name, string namePlural, string description, int weight) : base(name, namePlural, description, weight)
        {
            this.maxDamage = maxDamage;
            this.minDamage = minDamage;
        }
    }
}
