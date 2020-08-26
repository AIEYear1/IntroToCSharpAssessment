using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGThing
{
    public class LivingCreature
    {
        public Name name;
        public int currentHP;
        public int maximumHP;
        public bool knownNoun;
        public bool properNoun;

        public LivingCreature(Name name, int HP, bool knownNoun, bool properNoun)
        {
            this.name = name;
            this.currentHP = HP;
            this.maximumHP = HP;
            this.knownNoun = knownNoun;
            this.properNoun = properNoun;
        }
        public LivingCreature(int HP, bool knownNoun, bool properNoun)
        {
            this.currentHP = HP;
            this.maximumHP = HP;
            this.knownNoun = knownNoun;
            this.properNoun = properNoun;
        }

        public LivingCreature()
        {

        }
    }
}
