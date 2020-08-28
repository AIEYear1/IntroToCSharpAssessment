using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CRPGThing
{
    class Shop : NPC
    {
        public List<InventoryItem> stock = new List<InventoryItem>();
        int priceAugment;

        public Shop(Name name, string talkLine, List<InventoryItem> stockToAdd, int priceAugment) : base(name, talkLine)
        {
            this.priceAugment = priceAugment;


            for(int x = 0; x < stockToAdd.Count; x++)
            {
                bool itemAdded = false;
                for(int y = 0; y < stock.Count; y++)
                {
                    if(stock[y].details == stockToAdd[x].details)
                    {
                        stock[y].quantity++;
                        itemAdded = true;
                    }
                }

                if(!itemAdded)
                    stock.Add(stockToAdd[x]);
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
                    if(stock[x].details.value > stock[x + 1].details.value)
                    {
                        InventoryItem tmpHolder = stock[x + 1];
                        stock[x + 1] = stock[x];
                        stock[x] = tmpHolder;
                        swapped = true;
                    }
                }
                for (int x = stock.Count-1; x > 0 + iteration; x--)
                {
                    if (stock[x].details.value < stock[x - 1].details.value)
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

        public void Buy(Player player, InventoryItem itemToBuy)
        {
            if (player.gold < itemToBuy.details.value)
            {
                Utils.Add("Not enough gold");
                return;
            }
            if(!stock.Contains(itemToBuy))
            {
                Utils.Add("Shop doesn't have this item");
                return;
            }

            player.gold -= itemToBuy.details.value + priceAugment;
            player.AddItemToInventory(itemToBuy);

            if(itemToBuy.quantity > 1)
            {
                itemToBuy.quantity--;
                return;
            }

            stock.Remove(itemToBuy);
            SortByPrice();
        }

        public void Sell(Player player, InventoryItem itemToSell)
        {
            if (!player.Inventory.Contains(itemToSell))
            {
                Utils.Add("you don't have this item");
                return;
            }

            player.gold += itemToSell.details.value - priceAugment;
            player.RemoveItemFromInventory(itemToSell);

            for (int y = 0; y < stock.Count; y++)
            {
                if (stock[y].details == itemToSell.details)
                {
                    stock[y].quantity++;
                    return;
                }
            }

            stock.Add(itemToSell);
            SortByPrice();
        }
    }
}
