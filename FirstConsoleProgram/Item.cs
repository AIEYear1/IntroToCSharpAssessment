using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGNamespace
{
    public class Item
    {
        public string name = "", namePlural = "";
        public string description = "";
        public int value = 0;

        public Item(string name, string namePlural, string description, int value)
        {
            this.name = name;
            this.namePlural = namePlural;
            this.description = description;
            this.value = value;
        }
        public Item(string name, string namePlural, string description)
        {
            this.name = name;
            this.namePlural = namePlural;
            this.description = description;
        }

        public virtual void Look()
        {
            Utils.Add(name);
            Utils.Add(description);
        }

        public static implicit operator InventoryItem(Item i)
        {
            return new InventoryItem(i, 1);
        }
    }
}
