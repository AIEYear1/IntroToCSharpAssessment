using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGNamespace
{
    public abstract class Consumable : Item
    {
        public Consumable(int iD, string name, string namePlural, string description, int value) : base(iD, name, namePlural, description, value)
        {

        }

        public abstract void Effect(Player player);
    }
}
