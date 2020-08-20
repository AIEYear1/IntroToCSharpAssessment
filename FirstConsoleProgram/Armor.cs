using System;
using System.Collections.Generic;
using System.Text;

namespace FirstConsoleProgram
{
    public class Armor : Item
    {
        public int ac = 0;

        public Armor(int ac, string name, string namePlural, string description, int weight) : base(name, namePlural, description, weight)
        {
            this.ac = ac;
        }
    }
}
