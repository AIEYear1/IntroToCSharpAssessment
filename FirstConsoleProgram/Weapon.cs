using System;
using System.Collections.Generic;
using System.Text;
using RaylibWindowNamespace;

namespace CRPGNamespace
{
    public class Weapon : Item
    {
        public WeaponAttack weaponAttack;

        public Weapon(string name, string namePlural, string description, int value, WeaponAttack weaponAttack) : base(name, namePlural, description, value)
        {
            this.weaponAttack = weaponAttack;
        }
        public Weapon(string name, string namePlural, string description) : base(name, namePlural, description)
        {

        }

        public override void Look()
        {
            Utils.Add(name);
            Utils.Add($"\tAttack Power: {weaponAttack.minDamage}-{weaponAttack.maxDamage}");
            Utils.Add(description);
        }
    }
}
