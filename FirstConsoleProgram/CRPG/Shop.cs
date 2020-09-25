using System;
using System.Collections.Generic;
using System.Linq;

namespace CRPGNamespace
{
    /// <summary>
    /// Shop NPCs for buying and selling
    /// </summary>
    class Shop : QueryNPC
    {
        /// <summary>
        /// The NPCs shop stock
        /// </summary>
        public List<InventoryItem> stock = new List<InventoryItem>();
        /// <summary>
        /// Price multiplier for cost augementing
        /// </summary>
        readonly float priceAugment;

        /// Parameters
        /// <param name="name">Name of the NPC</param>
        /// <param name="talkLine">Line the NPC speaks when talked to</param>
        /// <param name="description">Desciption of the NPC</param>
        /// <param name="question">Question the NPC asks</param>
        /// <param name="priceAugment">Price multiplier for cost augement</param>
        public Shop(Name name, string talkLine, string description, string question, float priceAugment, bool knownNoun = false, bool properNoun = false) : base(name, talkLine, description, question, knownNoun, properNoun)
        {
            this.priceAugment = priceAugment;
        }

        /// <summary>
        /// What happens when you talk to the NPC
        /// </summary>
        public override void Talk()
        {
            base.Talk();
            ShowStock();
            Utils.Print();

            switch (Utils.AskQuestion(question))
            {
                //1st case "Buy", attempts to buy an item
                case string item when item.StartsWith("buy "):
                    item = item.Substring(4);
                    if (stock.SingleOrDefault(x => x.details.Name.ToLower() == item || x.details.NamePlural.ToLower() == item) != InventoryItem.Empty)
                    {
                        Buy(stock.SingleOrDefault(x => x.details.Name.ToLower() == item || x.details.NamePlural.ToLower() == item));
                        break;
                    }
                    Utils.Add("The shop doesn't have that");
                    break;
                //2nd case "Sell", attempts to sell an item
                case string item when item.StartsWith("sell "):
                    item = item.Substring(5);
                    if (Program.player.Inventory.SingleOrDefault(x => x.details.Name.ToLower() == item || x.details.NamePlural.ToLower() == item) != InventoryItem.Empty)
                    {
                        Sell(Program.player.Inventory.SingleOrDefault(x => x.details.Name.ToLower() == item || x.details.NamePlural.ToLower() == item));
                        break;
                    }
                    Utils.Add("you don't have that");
                    break;
                //3rd case "Back", exits out of the options
                case "back":
                    Utils.Add("If ya change yer mind talk to me again");
                    break;
                //Overflow
                default:
                    Utils.Add("If yer just gonna loiter around leave");
                    break;
            }
        }

        public override void Look()
        {
            base.Look();
            //Show stock items
            ShowStock();
        }

        /// <summary>
        /// Simply shows the items in the Shops inventory
        /// </summary>
        void ShowStock()
        {
            Utils.Add("shop Items:");
            for (int x = 0; x < stock.Count; x++)
            {
                int quant = stock[x].quantity;
                string name = (quant == 1) ? stock[x].details.Name : stock[x].details.NamePlural;

                if (stock[x].details is Weapon)
                {
                    Utils.Add($"\t{Utils.ColorText(name, TextColor.SALMON)} : {quant}");
                    continue;
                }
                if (stock[x].details is Armor)
                {
                    Utils.Add($"\t{Utils.ColorText(name, TextColor.LIGHTBLUE)} : {quant}");
                    continue;
                }
                if (stock[x].details is Consumable)
                {
                    Utils.Add($"\t{Utils.ColorText(name, TextColor.PINK)} : {quant}");
                    continue;
                }

                Utils.Add($"\t{Utils.ColorText(name, TextColor.GOLD)} : {quant}");
            }
        }

        /// <summary>
        /// Sorts the Shop inventory by price
        /// </summary>
        public void SortByPrice()
        {
            bool swapped = true;
            int iteration = 0;

            while (swapped)
            {
                swapped = false;

                for (int x = 0; x < stock.Count - iteration - 1; x++)
                {
                    if (stock[x].details.Value > stock[x + 1].details.Value)
                    {
                        InventoryItem tmpHolder = stock[x + 1];
                        stock[x + 1] = stock[x];
                        stock[x] = tmpHolder;
                        swapped = true;
                    }
                }
                for (int x = stock.Count - 1; x > 0 + iteration; x--)
                {
                    if (stock[x].details.Value < stock[x - 1].details.Value)
                    {
                        InventoryItem tmpHolder = stock[x - 1];
                        stock[x - 1] = stock[x];
                        stock[x] = tmpHolder;
                        swapped = true;
                    }
                }

                iteration++;
            }
        }

        /// <summary>
        /// Adds an item to the Shops stock
        /// </summary>
        /// <param name="itemToAdd">Item to add</param>
        public void AddItemToStock(InventoryItem itemToAdd)
        {
            for (int y = 0; y < stock.Count; y++)
            {
                if (stock[y] == itemToAdd)
                {
                    InventoryItem tmpItem = stock[y];
                    tmpItem.quantity++;
                    stock[y] = tmpItem;
                    return;
                }
            }

            stock.Add(itemToAdd);
        }
        /// <summary>
        /// Removes an item from the shops stock
        /// </summary>
        /// <param name="itemToRemove">Item to remove</param>
        public void RemoveItemFromStock(InventoryItem itemToRemove)
        {
            InventoryItem tmpItem = stock.Find(s => s == itemToRemove);
            if (tmpItem.quantity > 1)
            {
                tmpItem.quantity--;
                stock[stock.FindIndex(s => s == tmpItem)] = tmpItem;
                return;
            }

            stock.Remove(tmpItem);
        }

        /// <summary>
        /// Attempts to buy an item from the shop
        /// </summary>
        /// <param name="itemToBuy">Item to try to buy</param>
        public void Buy(InventoryItem itemToBuy)
        {
            if (Program.player.gold < itemToBuy.details.Value)
            {
                Utils.Add("Not enough gold");
                return;
            }
            if (!stock.Contains(itemToBuy))
            {
                Utils.Add("Shop doesn't have this item");
                return;
            }

            int cost = (int)(itemToBuy.details.Value * priceAugment);
            Program.player.gold -= cost;
            itemToBuy.quantity = 1;
            Program.player.AddItemToInventory(itemToBuy);
            Utils.Add($"You spend {Utils.ColorText(cost.ToString(), TextColor.YELLOW)} buy a {itemToBuy.details.Name}");

            RemoveItemFromStock(itemToBuy);

            SortByPrice();
        }

        /// <summary>
        /// Attempts to sell an item to the shop
        /// </summary>
        /// <param name="itemToSell">Item to try to sell</param>
        public void Sell(InventoryItem itemToSell)
        {
            if (!Program.player.Inventory.Contains(itemToSell))
            {
                Utils.Add("you don't have this item");
                return;
            }
            if (itemToSell.details is QuestItem)
            {
                Utils.Add("You can't sell quest items");
                return;
            }
            if (itemToSell == Program.player.currentWeapon || itemToSell == Program.player.currentArmor)
            {
                Utils.Add("You can't sell what you're wearing");
                return;
            }

            int cost = itemToSell.details.Value - (int)MathF.Abs(itemToSell.details.Value - (itemToSell.details.Value * priceAugment));
            Program.player.gold += cost;
            Program.player.RemoveItemFromInventory(itemToSell);
            Utils.Add($"You sell a {itemToSell.details.Name} and gain {Utils.ColorText(cost.ToString(), TextColor.YELLOW)} gold");

            AddItemToStock(itemToSell);

            SortByPrice();
        }
    }
}
