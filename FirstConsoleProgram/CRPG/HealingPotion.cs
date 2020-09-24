using System;

namespace CRPGNamespace
{
    /// <summary>
    /// Healing potion item type
    /// </summary>
    class HealingPotion : Consumable
    {
        public int amountToHeal;

        /// Parameters
        /// <param name="iD">Item ID for referencing</param>
        /// <param name="name">Item name</param>
        /// <param name="namePlural">Plural item name</param>
        /// <param name="description">Item description</param>
        /// <param name="value">Value of the item</param>
        /// <param name="amountToHeal">Amount of healing the potion does</param>
        public HealingPotion(int iD, string name, string namePlural, string description, int value, int amountToHeal) : base(iD, name, namePlural, description, value)
        {
            this.amountToHeal = amountToHeal;
        }

        /// <summary>
        /// Heals the player then removes the item
        /// </summary>
        /// <param name="player"></param>
        public override void Consume(Player player)
        {
            Utils.Add($"You use a {Name} healing {Utils.ColorText(amountToHeal.ToString(), TextColor.PURPLE)} health");
            player.currentHP = (int)MathF.Min(player.currentHP + amountToHeal, player.maximumHP);
            player.RemoveItemFromInventory(this);
        }

        public override void Look()
        {
            Utils.Add(Name);
            Utils.Add($"\tHeals {Utils.ColorText(amountToHeal.ToString(), TextColor.PURPLE)} health");
            Utils.Add(Description);
        }
    }
}
