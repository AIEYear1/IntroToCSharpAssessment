using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CRPGNamespace
{
    class Shop : QueryNPC
    {
        public List<InventoryItem> stock = new List<InventoryItem>();
        readonly float priceAugment;

        public Shop(Name name, string talkLine, string description, string question, List<InventoryItem> stockToAdd, float priceAugment, bool knownNoun = false, bool properNoun = false) : base(name, talkLine, description, question, knownNoun, properNoun)
        {
            this.priceAugment = priceAugment;


            for(int x = 0; x < stockToAdd.Count; x++)
            {
                AddItemToStock(stockToAdd[x]);
            }

            SortByPrice();
        }

        public override void Talk()
        {
            base.Talk();
            Utils.Add("shop Items:");
            for (int x = 0; x < stock.Count; x++)
            {
                if (stock[x].details is Weapon)
                {
                    Utils.Add($"\t{Utils.ColorText(stock[x].details.Name, TextColor.SALMON)} : {stock[x].quantity}");
                    continue;
                }
                if (stock[x].details is Armor)
                {
                    Utils.Add($"\t{Utils.ColorText(stock[x].details.Name, TextColor.LIGHTBLUE)} : {stock[x].quantity}");
                    continue;
                }
                if (stock[x].details is Consumable)
                {
                    Utils.Add($"\t{Utils.ColorText(stock[x].details.Name, TextColor.PINK)} : {stock[x].quantity}");
                    continue;
                }

                Utils.Add($"\t{Utils.ColorText(stock[x].details.Name, TextColor.GOLD)} : {stock[x].quantity}");
            }
            Utils.Print();
            switch (Utils.AskQuestion(question))
            {
                case string item when item.StartsWith("buy "):
                    item = item.Substring(4);
                    if(stock.SingleOrDefault(x => x.details.Name.ToLower() == item || x.details.NamePlural.ToLower() == item) != InventoryItem.Empty)
                    {
                        Buy(stock.SingleOrDefault(x => x.details.Name.ToLower() == item || x.details.NamePlural.ToLower() == item));
                        break;
                    }
                    Utils.Add("The shop doesn't have that");
                    break;
                case string item when item.StartsWith("sell "):
                    item = item.Substring(5);
                    if (Program.player.Inventory.SingleOrDefault(x => x.details.Name.ToLower() == item || x.details.NamePlural.ToLower() == item) != InventoryItem.Empty)
                    {
                        Sell(Program.player.Inventory.SingleOrDefault(x => x.details.Name.ToLower() == item || x.details.NamePlural.ToLower() == item));
                        break;
                    }
                    Utils.Add("you don't have that");
                    break;
                case "back":
                    Utils.Add("If ya change yer mind talk to me again");
                    break;
                default:
                    Utils.Add("If yer just gonna loiter around leave");
                    break;
            }
        }

        public void SortByPrice()
        {
            bool swapped = true;
            int iteration = 0;

            while (swapped)
            {
                swapped = false;

                for(int x = 0; x < stock.Count - iteration - 1; x++)
                {
                    if(stock[x].details.Value > stock[x + 1].details.Value)
                    {
                        InventoryItem tmpHolder = stock[x + 1];
                        stock[x + 1] = stock[x];
                        stock[x] = tmpHolder;
                        swapped = true;
                    }
                }
                for (int x = stock.Count-1; x > 0 + iteration; x--)
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

        public void Buy(InventoryItem itemToBuy)
        {
            if (Program.player.gold < itemToBuy.details.Value)
            {
                Utils.Add("Not enough gold");
                return;
            }
            if(!stock.Contains(itemToBuy))
            {
                Utils.Add("Shop doesn't have this item");
                return;
            }

            Program.player.gold -= (int)(itemToBuy.details.Value * priceAugment);
            itemToBuy.quantity = 1;
            Program.player.AddItemToInventory(itemToBuy);
            Utils.Add("You buy a " + itemToBuy.details.Name);

            RemoveItemFromStock(itemToBuy);

            SortByPrice();
        }

        public void Sell(InventoryItem itemToSell)
        {
            if (!Program.player.Inventory.Contains(itemToSell))
            {
                Utils.Add("you don't have this item");
                return;
            }
            if(itemToSell.details is QuestItem)
            {
                Utils.Add("You can't sell quest items");
                return;
            }
            if (itemToSell == Program.player.currentWeapon || itemToSell == Program.player.currentArmor)
            {
                Utils.Add("You can't sell what you're wearing");
                return;
            }

            Program.player.gold += itemToSell.details.Value - (int)MathF.Abs(itemToSell.details.Value - (itemToSell.details.Value * priceAugment));
            Program.player.RemoveItemFromInventory(itemToSell);
            Utils.Add("You sell a " + itemToSell.details.Name);

            AddItemToStock(itemToSell);

            SortByPrice();
        }
    }
}
