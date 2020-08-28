using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Transactions;

namespace CRPGThing
{ 
    class Program
    {
        #region color changing stuff
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleMode(IntPtr handle, out int mode);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int handle);
        #endregion

        public static bool running = true, initialized = false;
        public static Player player = new Player(10, 0, 50, 1, (Weapon)World.ItemByID(World.ITEM_ID_STICK), 
            (Armor)World.ItemByID(World.ITEM_ID_CLOTHES), World.LocationByID(World.LOCATION_ID_CLEARING), 15);

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
                case "help":
                    World.Help();
                    break;
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
                        break;
                    }
                    Utils.Add("Uh, alright then");
                    break;
                case string look when look.StartsWith("look "):
                    player.Look(look.Substring(5).Trim());
                    break;
                case "stats":
                    player.Stats();
                    break;
                case "inventory":
                case "i":
                    Utils.Add("Current Inventory: ");
                    foreach (InventoryItem invItem in player.Inventory)
                    {
                        Utils.Add($"\t{Utils.ColorText(invItem.details.name, (invItem.details is Weapon) ? Color.SALMON : ((invItem.details is Armor) ? Color.LIGHTBLUE : Color.GOLD))} : {invItem.quantity}");
                    }
                    break;
                case "quests":
                case "q":
                    Utils.Add("Current Quests: ");
                    foreach(Quest q in player.activeQuests)
                    {
                        Utils.Add("\t" + q.name);
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