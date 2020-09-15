﻿using System;
using System.Collections.Generic;
using System.Text;
using RaylibWindowNamespace;

namespace CRPGNamespace
{
    public class Monster : LivingCreature
    {
        public string description;
        public EnemyAttack enemyAttack;
        public int rewardXP = 0;
        public int rewardGold = 0;
        public List<LootItem> lootTable = new List<LootItem>();

        public Monster(Name name, string description, int HP, EnemyAttack enemyAttack, int rewardXP, int rewardGold, bool knownNoun = false, bool properNoun = false) : base(name, HP, knownNoun, properNoun)
        {
            this.Name = name;
            this.description = description;
            this.enemyAttack = enemyAttack;
            this.rewardXP = rewardXP;
            this.rewardGold = rewardGold;
        }

        public void LookAt()
        {
            KnownNoun = true;
            Utils.Add($"Stats for {Name.FullName}:");
            Utils.Add($"\tHP:\t\t{currentHP}/{maximumHP}");
            Utils.Add($"\tAttack power:\t{enemyAttack.minDamage}-{enemyAttack.maxDamage}");
            Utils.Add("Attack: " + enemyAttack.description);
            Utils.Add(description);
        }

        public override void TakeDamage(int damage = 0)
        {
            damage = Utils.NumberBetween(Program.player.CurrentMinDamage, Program.player.CurrentMaxDamage);
            currentHP -= damage;
            Utils.Add($"You hit {Utils.PrefixNoun(Name.FullName, ProperNoun, KnownNoun, TextColor.RED)} for {Utils.ColorText(damage.ToString(), TextColor.BLUE)} damage!");
            if (currentHP <= 0)
            {
                Die(Program.player);
                Program.combatWindow.EndAttack();
            }
        }

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
                    Utils.Add($"You loot {inventoryItem.quantity} {Utils.ColorText(inventoryItem.details.Name, (inventoryItem.details is Weapon) ? TextColor.SALMON: ((inventoryItem.details is Armor) ? TextColor.LIGHTBLUE : TextColor.GOLD))}");
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
    }
}
