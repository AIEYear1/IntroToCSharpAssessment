using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace CRPGNamespace
{
    public struct InventoryItem
    {
        public Item details;
        public int quantity;

        public InventoryItem(Item details, int quantity)
        {
            this.details = details;
            this.quantity = quantity;
        }

        public static readonly InventoryItem Empty = new InventoryItem();

        public static bool operator ==(InventoryItem a, InventoryItem b)
        {
            return a.details == b.details;
        }
        public static bool operator !=(InventoryItem a, InventoryItem b)
        {
            return !(a == b);
        }
    }
}
