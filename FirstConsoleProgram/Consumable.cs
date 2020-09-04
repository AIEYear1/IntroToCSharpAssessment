using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGThing
{
    public abstract class Consumable : Item
    {
        public Consumable(string name, string namePlural, string description, int value) : base(name, namePlural, description, value)
        {

        }

        public abstract void Effect(Player player);
    }
}
