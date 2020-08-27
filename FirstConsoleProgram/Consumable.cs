using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGThing
{
    public abstract class Consumable : Item
    {
        public Consumable(string name, string namePlural, string description, int weight) : base(name, namePlural, description, weight)
        {

        }

        public abstract void Effect(Player player);
    }
}
