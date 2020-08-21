using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGThing
{
    public class InventoryItem
    {
        public Item details;
        public int quantity;

        public InventoryItem(Item details, int quantity)
        {
            this.details = details;
            this.quantity = quantity;
        }
    }
}
