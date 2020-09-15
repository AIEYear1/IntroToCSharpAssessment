using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGNamespace
{
    public abstract class LivingCreature
    {
        public Name Name;
        public int currentHP;
        public int maximumHP;
        public bool KnownNoun;
        public bool ProperNoun;

        public LivingCreature(Name name, int HP, bool knownNoun, bool properNoun)
        {
            this.Name = name;
            this.currentHP = HP;
            this.maximumHP = HP;
            this.KnownNoun = knownNoun;
            this.ProperNoun = properNoun;
        }
        public LivingCreature(int HP, bool knownNoun, bool properNoun)
        {
            this.currentHP = HP;
            this.maximumHP = HP;
            this.KnownNoun = knownNoun;
            this.ProperNoun = properNoun;
        }

        public LivingCreature()
        {

        }

        public abstract void TakeDamage(int damage);
    }
}
