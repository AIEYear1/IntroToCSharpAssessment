using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGThing
{
    public class Monster : LivingCreature
    {
        public Name name = null;
        public string description;
        public int maximumDamage = 0;
        public int minimumDamage = 0;
        public int rewardXP = 0;
        public int rewardGold = 0;
        public List<LootItem> lootTable = new List<LootItem>();

        public Monster(Name name, string description, int HP, int maximumDamage, int minimumDamage, int rewardXP, int rewardGold) : base(HP)
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
            Utils.Add($"\nStats for {name.FullName}:");
            Utils.Add($"\tHP:\t\t{currentHP}/{maximumHP}");
            Utils.Add($"\tAttack power:\t{minimumDamage}-{maximumDamage}");
            Utils.Add(description);
        }

        public void Attack(Player player)
        {
            int damage = (int)MathF.Max((RandomNumberGenerator.NumberBetween(minimumDamage, maximumDamage)) - (player.currentArmor.ac + player.baseAc), 0);
            player.currentHP -= damage;
            Utils.Add($"You were hit by {name.FullName} for {damage} damage!");

            if (player.currentHP <= 0)
            {
                Utils.Add(player.name.FullName + " has died!");
                Program.running = false;
            }
        }

        public void Die(Player player)
        {
            player.gold += rewardGold;
            player.EarnXP(rewardXP);
            player.currentLocation.monsterLivingHere = null;

            Utils.Add(name.FullName + " has died");
            Utils.Add($"You gained {rewardGold} gold");
            Utils.Add($"You earned {rewardXP} XP");

            // Get random loot items from the monster
            List<InventoryItem> lootedItems = new List<InventoryItem>();

            // Add items to the lootedItems list, comparing a random number to the drop percentage
            foreach (LootItem lootItem in lootTable)
            {
                if (RandomNumberGenerator.NumberBetween(1, 100) <= lootItem.dropPercentage)
                {
                    lootedItems.Add(new InventoryItem(lootItem.details, 1));
                }
            }

            // If no items were randomly selected, then add the default loot item(s).
            if (lootedItems.Count == 0)
            {
                foreach (LootItem lootItem in lootTable)
                {
                    if (lootItem.isDefault)
                    {
                        lootedItems.Add(new InventoryItem(lootItem.details, 1));
                    }
                }
            }

            // Add the looted items to the player's inventory
            foreach (InventoryItem inventoryItem in lootedItems)
            {
                player.AddItemToInventory(inventoryItem);

                if (inventoryItem.quantity == 1)
                {
                    Utils.Add($"You loot {inventoryItem.quantity} {inventoryItem.details.name}");
                }
                else
                {
                    Utils.Add($"You loot {inventoryItem.quantity} {inventoryItem.details.namePlural}");
                }
            }

            if (this is QuestMonster)
            {
                (this as QuestMonster).CallQuest();
            }
        }
    }
}
