using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using RaylibWindowNamespace;

namespace CRPGNamespace
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

        public static Window combatWindow = new Window();

        public static string fileToLoad = "";
        public static bool loadSave = false;

        static void Main()
        {
            player.AddItemToInventory(new InventoryItem(World.ItemByID(World.ITEM_ID_STICK), 1));
            player.AddItemToInventory(new InventoryItem(World.ItemByID(World.ITEM_ID_CLOTHES), 1));

            Console.Clear();

            while (!initialized)
            {
                while (loadSave)
                {
                    if (AttemptLoad(fileToLoad))
                        initialized = true;
                    Utils.Print();
                }
                if (initialized)
                    break;
                switch (Utils.AskQuestion("new or load?"))
                {
                    case "new":
                        Initialize();
                        break;
                    case string load when load.StartsWith("load"):
                        fileToLoad = "";
                        if (load.Length > 5)
                            fileToLoad = load.Substring(5);

                        loadSave = true;
                        break;
                    case "quit":
                        initialized = true;
                        running = false;
                        break;
                }
            }

            //Loop Start
            while (running)
            {
                while (loadSave)
                {
                    AttemptLoad(fileToLoad);
                    Utils.Print();
                }

                while (!combatWindow.WindowHidden)
                {
                    combatWindow.Run();
                }

                ParseInput(Utils.GetInput());
            }

            combatWindow.Close();
        }

        static void Initialize()
        {
            player.SetName();

            player.MoveTo(player.home);
            if (running)
                Utils.Print();

            initialized = true;
        }

        static bool attempted = false;
        static bool AttemptLoad(string fileToAttmept)
        {
            string filePath = @".\";
            string[] files = Directory.GetFiles(@".\", "*.save");

            if(files.Length == 0)
            {
                Utils.Add("No save files found");
                loadSave = false;
                return false;
            }

            string input = fileToAttmept;

            if (fileToAttmept == "" || attempted)
            {
                for (int x = 0; x < files.Length; x++)
                {
                    Utils.Add("\t" + files[x].Substring(filePath.Length).Trim());
                }
                Utils.Print();

                input = Utils.AskQuestion("which save do you wish to load?");

                switch (input)
                {
                    case "quit":
                        running = false;
                        loadSave = false;
                        return true;
                    case "back":
                        loadSave = false;
                        attempted = false;
                        return false;
                    case "help":
                        Utils.Add("back to return to game, quit to leave");
                        return false;
                    case string file when file.StartsWith("delete "):
                        file = file.Substring(7);
                        for (int x = 0; x < files.Length; x++)
                        {
                            if (file == files[x].Substring(filePath.Length).Trim().ToLower() || file == files[x].Substring(filePath.Length).Trim().ToLower().Split('.')[0])
                            {
                                File.Delete(files[x]);
                                Utils.Add("save successfully deleted");
                                return false;
                            }
                        }
                        return false;
                }
            }

            for (int x = 0; x < files.Length; x++)
            {
                if(input == files[x].Substring(filePath.Length).Trim().ToLower() || input == files[x].Substring(filePath.Length).Trim().ToLower().Split('.')[0])
                {
                    Player.Load(files[x].Substring(filePath.Length).Split('.')[0]);
                    loadSave = false;
                    attempted = false;
                    Utils.Add("save successfully loaded");
                    return true;
                }
            }

            Utils.Add("no save file of that name found");
            attempted = true;
            return false;
        }

        //TODO: Implement NPC interaction and shopping
        static void ParseInput(string input)
        {
            switch (input)
            {
                case "help":                                            //1st case "help"
                    World.Help();
                    break;
                case "who am i":                                        //2nd case "who am i"
                    if (player.Name.FirstName != "")
                    {
                        Utils.Add(player.Name.FullName);
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
                case string look when look.StartsWith("look "):         //3rd case "look"
                    player.Look(look.Substring(5).Trim());
                    break;
                case "stats":                                           //4th case "stats"
                    player.Stats();
                    break;
                case "inventory":                                       //5th case "inventory" "i"
                case "i":
                    Utils.Add("Current Inventory: ");
                    foreach (InventoryItem invItem in player.Inventory)
                    {
                        Utils.Add($"\t{Utils.ColorText(invItem.details.Name, (invItem.details is Weapon) ? TextColor.SALMON : ((invItem.details is Armor) ? TextColor.LIGHTBLUE : TextColor.GOLD))} : {invItem.quantity}");
                    }
                    break;
                case "quests":                                          //6th case "quest" "q"
                case "q":
                    Utils.Add("Current Quests: ");
                    foreach(Quest q in player.activeQuests)
                    {
                        Utils.Add("\t" + q.name);
                    }
                    break;
                case string move when move.StartsWith("move "):         //7th case "move"
                    MovePlayer(move.Substring(5).Trim());
                    break;
                case string save when save.StartsWith("save "):         //8th case "save"
                    player.Save(save.Substring(5).Trim());
                    break;
                case string load when load.StartsWith("load"):          //9th case "load"
                    fileToLoad = "";
                    if (load.Length > 5)
                        fileToLoad = load.Substring(5);

                    loadSave = true;
                    break;
                case string equip when equip.StartsWith("equip "):      //10th case "equip"
                    string tmpEquipInput = equip.Substring(6).Trim();
                    player.EquipItem(tmpEquipInput);
                    break;
                case "attack":                                          //11th case "attack"
                    player.Attack(player.currentLocation.monsterLivingHere);
                    break;
                case "quit":                                            //12th case "quit"
                    running = false;
                    break;
                default:                                                //Overflow
                    Utils.Add("I- I don- I don't understand, type 'help' for commands");
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