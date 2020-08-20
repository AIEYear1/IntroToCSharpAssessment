using System;
using System.Collections.Generic;
using System.Text;

namespace FirstConsoleProgram
{
    public class LivingCreature
    {
        public int currentHP;
        public int maximumHP;

        public LivingCreature(int HP)
        {
            this.currentHP = HP;
            this.maximumHP = HP;
        }

        public LivingCreature()
        {

        }
    }
}
