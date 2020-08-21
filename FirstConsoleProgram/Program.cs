using System;
using System.Collections.Generic;
using System.Linq;

namespace CRPGThing
{ 
    class Program
    {
        public static bool running = true;
        public static Player player = new Player(null, 0, 0, 20, 1, (Weapon)World.ItemByID(World.ITEM_ID_STICK), 
            (Armor)World.ItemByID(World.ITEM_ID_CLOTHES), World.LocationByID(World.LOCATION_ID_STARTINGLOCATION), 15);

        static void Main()
        {
            player.AddItemToInventory(new InventoryItem(World.ItemByID(World.ITEM_ID_STICK), 1));
            player.AddItemToInventory(new InventoryItem(World.ItemByID(World.ITEM_ID_CLOTHES), 1));

            player.SetName();

            player.MoveTo(player.home);
            Utils.Print();

            //Loop Start
            while (running)
            {
                ParseInput(Utils.GetInput());
            }
        }

        static void ParseInput(string input)
        {
            switch (input)
            {
                case "who am i":                //1st case "who am i"
                    if (player.name.FirstName != "")
                    {
                        Utils.Add(player.name.FullName);
                        break;
                    }

                    input = Utils.AskQuestion("You did not give us your name would you like to? Yes or No");

                    if (input == "yes")
                    {
                        player.SetName();
                    }
                    else
                    {
                        Utils.Add("Uh, alright then");
                    }
                    break;
                case string look when look.StartsWith("look "):
                    player.Look(look.Substring(5).Trim());
                    break;
                case "stats":
                    player.Stats();
                    break;
                case "inventory":
                case "i":
                    Utils.Add("\nCurrent Inventory: ");
                    foreach (InventoryItem invItem in player.Inventory)
                    {
                        Utils.Add($"\t{invItem.details.name} : {invItem.quantity}");
                    }
                    break;
                case string move when move.StartsWith("move "):
                    MovePlayer(move.Substring(5).Trim());
                    break;
                case string equip when equip.StartsWith("equip "):
                    string tmpEquipInput = equip.Substring(6).Trim();
                    player.EquipItem(tmpEquipInput);
                    break;
                case "attack":
                    player.Attack(player.currentLocation.monsterLivingHere);
                    break;
                case "quit":                    //2nd case "quit"
                    running = false;
                    break;
                default:                        //Overflow
                    Utils.Add("I- I don- I don't understand");
                    break;
            }

            Utils.Print();
        }

        static void MovePlayer(string arg)
        {
            switch (arg)
            {
                case "north":
                case "up":
                case "n":
                    player.MoveNorth();
                    break;
                case "east":
                case "right":
                case "e":
                    player.MoveEast();
                    break;
                case "south":
                case "down":
                case "s":
                    player.MoveSouth();
                    break;
                case "west":
                case "left":
                case "w":
                    player.MoveWest();
                    break;
                default:
                    Utils.Add("that's not a direction");
                    break;
            }
        }
    }
}