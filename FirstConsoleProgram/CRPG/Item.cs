namespace CRPGNamespace
{
    /// <summary>
    /// Base class for all Items
    /// </summary>
    public class Item
    {
        /// <summary>
        /// ID reference for finding specific items
        /// </summary>
        public int ID = 0;
        /// <summary>
        /// Name of the item
        /// </summary>
        public string Name = "";
        /// <summary>
        /// Plural name of the item
        /// </summary>
        public string NamePlural = "";
        /// <summary>
        /// Description of the item
        /// </summary>
        public string Description = "";
        /// <summary>
        /// Value of the item
        /// </summary>
        public int Value = 0;

        /// Parameters
        /// <param name="iD">Item ID for referencing</param>
        /// <param name="name">Item name</param>
        /// <param name="namePlural">Plural item name</param>
        /// <param name="description">Item description</param>
        /// <param name="value">Value of the item</param>
        public Item(int iD, string name, string namePlural, string description, int value)
        {
            ID = iD;
            Name = name;
            NamePlural = namePlural;
            Description = description;
            Value = value;
        }

        /// <summary>
        /// Look command for Item shares name and description
        /// </summary>
        public virtual void Look()
        {
            Utils.Add(Name);
            Utils.Add(Description);
        }

        public static implicit operator InventoryItem(Item i)
        {
            return new InventoryItem(i, 1);
        }
    }
}
