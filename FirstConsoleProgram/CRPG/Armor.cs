namespace CRPGNamespace
{
    /// <summary>
    /// Armor item type
    /// </summary>
    public class Armor : Item
    {
        /// <summary>
        /// how much damage this armor blocks
        /// </summary>
        public int ac = 0;

        /// Parameters
        /// <param name="iD">Item ID for referencing</param>
        /// <param name="name">Item name</param>
        /// <param name="namePlural">Plural item name</param>
        /// <param name="description">Item description</param>
        /// <param name="value">Value of the item</param>
        /// <param name="ac">Armor value</param>
        public Armor(int iD, string name, string namePlural, string description, int value, int ac) : base(iD, name, namePlural, description, value)
        {
            this.ac = ac;
        }

        /// <summary>
        /// Look command for armor shares name ac and description
        /// </summary>
        public override void Look()
        {
            Utils.Add(Name);
            Utils.Add($"\tProtection Level: {ac}");
            Utils.Add(Description);
        }
    }
}
