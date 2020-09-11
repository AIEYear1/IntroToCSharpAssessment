using CsvHelper.Configuration;
using RaylibWindowNamespace;

namespace CRPGNamespace
{
    public class Weapon : Item
    {
        public WeaponAttack WeaponAttack;

        public Weapon(int iD, string name, string namePlural, string description, int value, WeaponAttack weaponAttack) : base(iD, name, namePlural, description, value)
        {
            this.WeaponAttack = weaponAttack;
        }

        public override void Look()
        {
            Utils.Add(Name);
            Utils.Add($"\tAttack Power: {WeaponAttack.minDamage}-{WeaponAttack.maxDamage}");
            Utils.Add("Attack: " + WeaponAttack.description);
            Utils.Add(Description);
        }
    }
}

