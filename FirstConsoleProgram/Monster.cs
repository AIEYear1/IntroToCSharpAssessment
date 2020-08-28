using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGThing
{
    public class Monster : LivingCreature
    {
        public string description;
        public int maximumDamage = 0;
        public int minimumDamage = 0;
        public int rewardXP = 0;
        public int rewardGold = 0;
        public List<LootItem> lootTable = new List<LootItem>();

        public Monster(Name name, string description, int HP, int maximumDamage, int minimumDamage, int rewardXP, int rewardGold, bool knownNoun = false, bool properNoun = false) : base(name, HP, knownNoun, properNoun)
        {
            this.name = name;
            this.description = description;
            this.maximumDamage = maximumDamage;
            this.minimumDamage = minimumDamage;
            this.rewardXP = rewardXP;
            this.rewardGold = rewardGold;
        }

        public void LookAt()
        {
            knownNoun = true;
            Utils.Add($"Stats for {name.FullName}:");
            Utils.Add($"\tHP:\t\t{currentHP}/{maximumHP}");
            Utils.Add($"\tAttack power:\t{minimumDamage}-{maximumDamage}");
            Utils.Add(description);
        }

        public void Attack(Player player)
        {
            int damage = (int)MathF.Max((RandomNumberGenerator.NumberBetween(minimumDamage, maximumDamage)) - (player.CurrentAc), 0);
            player.currentHP -= damage;
            Utils.Add($"You were hit by {Utils.PrefixNoun(name.FullName, properNoun, knownNoun, Color.RED)} for {Utils.ColorText(damage.ToString(), Color.BLUE)} damage!");

            if (player.currentHP <= 0)
            {
                Utils.Add(Utils.ColorText(player.name.FullName + " has died!", Color.DARKRED));
                player.MoveTo(player.home, true);
            }
        }

        public void Die(Player player)
        {
            player.gold += rewardGold;
            player.EarnXP(rewardXP);
            player.currentLocation.monsterLivingHere = null;

            Utils.Add(Utils.PrefixNoun(name.FullName, properNoun, knownNoun, Color.RED) + " has died");
            Utils.Add($"You gained {Utils.ColorText(rewardGold.ToString(), Color.YELLOW)} gold");
            Utils.Add($"You earned {Utils.ColorText(rewardXP.ToString(), Color.GREEN)} XP");

            // Get random loot items from the monster
            List<InventoryItem> lootedItems = new List<InventoryItem>();

            // Add items to the lootedItems list, comparing a random number to the drop percentage
            foreach (LootItem lootItem in lootTable)
            {
                if (RandomNumberGenerator.NumberBetween(1, 100) <= lootItem.dropPercentage)
                {
                    lootedItems.Add(lootItem.details);
                }
            }

            // If no items were randomly selected, then add the default loot item(s).
            if (lootedItems.Count == 0)
            {
                foreach (LootItem lootItem in lootTable)
                {
                    if (lootItem.isDefault)
                    {
                        lootedItems.Add(lootItem.details);
                    }
                }
            }

            // Add the looted items to the player's inventory
            foreach (InventoryItem inventoryItem in lootedItems)
            {
                player.AddItemToInventory(inventoryItem);

                if (inventoryItem.quantity == 1)
                {
                    Utils.Add($"You loot {inventoryItem.quantity} {Utils.ColorText(inventoryItem.details.name, (inventoryItem.details is Weapon) ? Color.SALMON: ((inventoryItem.details is Armor) ? Color.LIGHTBLUE : Color.GOLD))}");
                }
                else
                {
                    Utils.Add($"You loot {inventoryItem.quantity} {Utils.ColorText(inventoryItem.details.namePlural, (inventoryItem.details is Weapon) ? Color.SALMON : ((inventoryItem.details is Armor) ? Color.LIGHTBLUE : Color.GOLD))}");
                }
            }

            if (this is QuestMonster)
            {
                (this as QuestMonster).CallQuest();
            }
        }
    }
}
