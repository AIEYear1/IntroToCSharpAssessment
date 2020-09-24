namespace CRPGNamespace
{
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
    /// <summary>
    /// Items stored with quantities
    /// </summary>
    public struct InventoryItem
    {
        public Item details;
        public int quantity;

        public InventoryItem(Item details, int quantity)
        {
            this.details = details;
            this.quantity = quantity;
        }

        /// <summary>
        /// Empty InventoryItem for null checks
        /// </summary>
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
