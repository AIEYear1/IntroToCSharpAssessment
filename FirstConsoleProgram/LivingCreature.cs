using System;
using System.Collections.Generic;
using System.Text;

namespace FirstConsoleProgram
{
    public class LivingCreature
    {
        public int currentHP;
        public int maximumHP;

        public LivingCreature(int currentHP, int maximumHP)
        {
            this.currentHP = currentHP;
            this.maximumHP = maximumHP;
        }

        public LivingCreature()
        {

        }
    }
}
