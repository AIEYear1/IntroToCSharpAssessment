using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGThing
{
    public class LootItem
    {
        public Item details;
        public int dropPercentage;
        public bool isDefault;

        public LootItem(Item details, int dropPercentage, bool isDefault)
        {
            this.details = details;
            this.dropPercentage = dropPercentage;
            this.isDefault = isDefault;
        }
    }
}
