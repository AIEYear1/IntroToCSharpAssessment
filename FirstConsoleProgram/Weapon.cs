using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGThing
{
    public class Weapon : Item
    {
        public int maxDamage = 0;
        public int minDamage = 0;

        public Weapon(string name, string namePlural, string description, int weight, int maxDamage, int minDamage) : base(name, namePlural, description, weight)
        {
            this.maxDamage = maxDamage;
            this.minDamage = minDamage;
        }
        public Weapon(string name, string namePlural, string description) : base(name, namePlural, description)
        {

        }
    }
}
