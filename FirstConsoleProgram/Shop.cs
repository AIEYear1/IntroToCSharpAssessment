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
        readonly int priceAugment;

        public Shop(Name name, string talkLine, string description, string question, List<InventoryItem> stockToAdd, int priceAugment, bool knownNoun = false, bool properNoun = false) : base(name, talkLine, description, question, knownNoun, properNoun)
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
            Utils.Print();
            switch (Utils.AskQuestion(question))
            {
                case string item when item.StartsWith("buy "):
                    item = item.Substring(4);
                    if(stock.SingleOrDefault(x => x.details.Name.ToLower() == item || x.details.NamePlural.ToLower() == item) != null)
                    {
                        Buy(stock.SingleOrDefault(x => x.details.Name.ToLower() == item || x.details.NamePlural.ToLower() == item));
                        break;
                    }
                    Utils.Add("The shop doesn't have that");
                    break;
                case string item when item.StartsWith("sell "):
                    item = item.Substring(5);
                    if (Program.player.Inventory.SingleOrDefault(x => x.details.Name.ToLower() == item || x.details.NamePlural.ToLower() == item) != null)
                    {
                        Sell(Program.player.Inventory.SingleOrDefault(x => x.details.Name.ToLower() == item || x.details.NamePlural.ToLower() == item));
                        break;
                    }
                    Utils.Add("you don't have that");
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
                if (stock[y].details == itemToAdd.details)
                {
                    stock[y].quantity++;
                    return;
                }
            }

            stock.Add(itemToAdd);
        }
        public void RemoveItemFromStock(InventoryItem itemToRemove)
        {
            if (itemToRemove.quantity > 1)
            {
                itemToRemove.quantity--;
                return;
            }

            stock.Remove(itemToRemove);
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

            Program.player.gold -= itemToBuy.details.Value + priceAugment;
            Program.player.AddItemToInventory(itemToBuy);

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

            Program.player.gold += itemToSell.details.Value - priceAugment;
            Program.player.RemoveItemFromInventory(itemToSell);

            AddItemToStock(itemToSell);

            SortByPrice();
        }
    }
}
