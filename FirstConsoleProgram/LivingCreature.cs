using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGThing
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
