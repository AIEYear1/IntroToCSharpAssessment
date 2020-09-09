using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGNamespace
{
    public class Armor : Item
    {
        public int ac = 0;

        public Armor(string name, string namePlural, string description, int value, int ac) : base(name, namePlural, description, value)
        {
            this.ac = ac;
        }
        public Armor(string name, string namePlural, string description) : base(name, namePlural, description)
        {

        }

        public override void Look()
        {
            Utils.Add(name);
            Utils.Add($"\tProtection Level: {ac}");
            Utils.Add(description);
        }
    }
}
