using RaylibWindowNamespace;

namespace CRPGNamespace
{
    /// <summary>
    /// Weapon item type
    /// </summary>
    public class Weapon : Item
    {
        /// <summary>
        /// Attack the weapon makes
        /// </summary>
        public WeaponAttack WeaponAttack;

        /// Parameters
        /// <param name="iD">Item ID for referencing</param>
        /// <param name="name">Item name</param>
        /// <param name="namePlural">Plural item name</param>
        /// <param name="description">Item description</param>
        /// <param name="value">Value of the item</param>
        /// <param name="weaponAttack"></param>
        public Weapon(int iD, string name, string namePlural, string description, int value, WeaponAttack weaponAttack) : base(iD, name, namePlural, description, value)
        {
            this.WeaponAttack = weaponAttack;
        }

        /// <summary>
        /// Look command for Weapon shares name, attack power, attack description and description
        /// </summary>
        public override void Look()
        {
            Utils.Add(Name);
            Utils.Add($"\tAttack Power: {WeaponAttack.minDamage}-{WeaponAttack.maxDamage}");
            Utils.Add("Attack: " + WeaponAttack.description);
            Utils.Add(Description);
        }
    }
}

