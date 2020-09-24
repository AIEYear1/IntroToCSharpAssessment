using RaylibWindowNamespace;
using System.Collections.Generic;

namespace CRPGNamespace
{
    /// <summary>
    /// Monsters that the player fights
    /// </summary>
    public class Monster : LivingCreature
    {
        /// <summary>
        /// Description of the monster
        /// </summary>
        public string description;
        /// <summary>
        /// Attack the enmy uses
        /// </summary>
        public EnemyAttack enemyAttack;
        /// <summary>
        /// XP earned from defeating the monster
        /// </summary>
        public int rewardXP = 0;
        /// <summary>
        /// Gold earned from defeating the monster
        /// </summary>
        public int rewardGold = 0;
        /// <summary>
        /// List of all possible loot items you can get from this monster
        /// </summary>
        public LootItem[] lootTable;

        /// Parameters
        /// <param name="name">Name of the monster</param>
        /// <param name="description">Description of the monster</param>
        /// <param name="HP">Health of the monster</param>
        /// <param name="enemyAttack">Attack the enemy uses</param>
        /// <param name="rewardXP">XP earned from defeating the monster</param>
        /// <param name="rewardGold">Gold earned from defeating the monster</param>
        public Monster(Name name, string description, int HP, EnemyAttack enemyAttack, int rewardXP, int rewardGold, bool knownNoun = false, bool properNoun = false) : base(name, HP, knownNoun, properNoun)
        {
            this.Name = name;
            this.description = description;
            this.enemyAttack = enemyAttack;
            this.rewardXP = rewardXP;
            this.rewardGold = rewardGold;
        }

        /// <summary>
        /// Text which pops up when you look at a monster
        /// </summary>
        public void LookAt()
        {
            KnownNoun = true;
            Utils.Add($"Stats for {Name.FullName}:");
            Utils.Add($"\tHP:\t\t{currentHP}/{maximumHP}");
            Utils.Add($"\tAttack power:\t{enemyAttack.minDamage}-{enemyAttack.maxDamage}");
            Utils.Add("Attack: " + enemyAttack.description);
            Utils.Add(description);
        }

        /// <summary>
        /// Monster taking damage
        /// </summary>
        public override int TakeDamage()
        {
            int damage = Utils.NumberBetween(Program.player.CurrentMinDamage, Program.player.CurrentMaxDamage);
            currentHP -= damage;
            Utils.Add($"You hit {Utils.PrefixNoun(Name.FullName, ProperNoun, KnownNoun, TextColor.RED)} for {Utils.ColorText(damage.ToString(), TextColor.BLUE)} damage!");
            if (currentHP <= 0)
            {
                Die(Program.player);
                Program.combatWindow.EndAttack();
            }
            return damage;
        }

        /// <summary>
        /// Monster dieing and giving loot to the player
        /// </summary>
        public void Die(Player player)
        {
            player.gold += rewardGold;
            player.EarnXP(rewardXP);
            player.currentLocation.monsterLivingHere = null;

            Utils.Add(Utils.PrefixNoun(Name.FullName, ProperNoun, KnownNoun, TextColor.RED) + " has died");
            Utils.Add($"You gained {Utils.ColorText(rewardGold.ToString(), TextColor.YELLOW)} gold");
            Utils.Add($"You earned {Utils.ColorText(rewardXP.ToString(), TextColor.GREEN)} XP");

            // Get random loot items from the monster
            List<InventoryItem> lootedItems = new List<InventoryItem>();

            // Add items to the lootedItems list, comparing a random number to the drop percentage
            foreach (LootItem lootItem in lootTable)
            {
                if (Utils.NumberBetween(1, 100) <= lootItem.dropPercentage)
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
                    Utils.Add($"You loot {inventoryItem.quantity} {Utils.ColorText(inventoryItem.details.Name, (inventoryItem.details is Weapon) ? TextColor.SALMON : ((inventoryItem.details is Armor) ? TextColor.LIGHTBLUE : TextColor.GOLD))}");
                }
                else
                {
                    Utils.Add($"You loot {inventoryItem.quantity} {Utils.ColorText(inventoryItem.details.NamePlural, (inventoryItem.details is Weapon) ? TextColor.SALMON : ((inventoryItem.details is Armor) ? TextColor.LIGHTBLUE : TextColor.GOLD))}");
                }
            }

            if (this is QuestMonster)
            {
                (this as QuestMonster).CallQuest();
            }
        }

        /// <summary>
        /// Override for abstract class NOT IMPLEMENTED
        /// </summary>
        public override void TakeDamage(int damage)
        {
            throw new System.NotImplementedException();
        }
    }
}
