using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGThing
{
    public class Armor : Item
    {
        public int ac = 0;

        public Armor(string name, string namePlural, string description, int weight, int ac) : base(name, namePlural, description, weight)
        {
            this.ac = ac;
        }
        public Armor(string name, string namePlural, string description) : base(name, namePlural, description)
        {

        }
    }
}
