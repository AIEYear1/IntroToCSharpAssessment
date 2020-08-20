using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGThing
{
    public class Potion : Item
    {
        public int amountHealed = 0;

        public Potion(int amountHealed, string name, string namePlural, string description, int weight) : base(name, namePlural, description, weight)
        {
            this.amountHealed = amountHealed;
        }
    }
}
