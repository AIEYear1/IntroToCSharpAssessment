using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGThing
{
    public class Item
    {
        public string name = "", namePlural = "";
        public string description = "";
        public int weight = 0;

        public Item(string name, string namePlural, string description, int weight)
        {
            this.name = name;
            this.namePlural = namePlural;
            this.description = description;
            this.weight = weight;
        }
        public Item()
        {

        }
    }
}
