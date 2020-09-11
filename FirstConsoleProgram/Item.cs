using CsvHelper.Configuration;

namespace CRPGNamespace
{
    public class Item
    {
        public int ID = 0;
        public string Name = ""; 
        public string NamePlural = "";
        public string Description = "";
        public int Value = 0;

        public Item(int iD, string name, string namePlural, string description, int value)
        {
            ID = iD;
            Name = name;
            NamePlural = namePlural;
            Description = description;
            Value = value;
        }

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
