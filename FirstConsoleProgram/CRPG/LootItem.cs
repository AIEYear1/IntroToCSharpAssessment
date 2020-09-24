namespace CRPGNamespace
{
    /// <summary>
    /// Container for monster loot items
    /// </summary>
    public struct LootItem
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
