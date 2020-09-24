using RaylibWindowNamespace;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace CRPGNamespace
{
    /// <summary>
    /// Main Class
    /// </summary>
    class Program
    {
        #region color changing stuff
        //I don't know what any of this does, I just think it would be a bad idea to remove this
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleMode(IntPtr handle, out int mode);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int handle);
        #endregion

        //two bools which are used in Main to manage the two important loops
        public static bool running = true, initialized = false;

        /// <summary>
        /// The main player, essential to gameplay, retainer of all relevant data
        /// </summary>
        public static Player player = new Player(10, 0, 50, 1, (Weapon)World.ItemByID((int)ItemIDs.STICK),
            (Armor)World.ItemByID((int)ItemIDs.CLOTHES), World.LocationByID((int)LocationIDs.CLEARING), 15);

        /// <summary>
        /// The main Combat Window, where attacking will take place
        /// </summary>
        public static Window combatWindow = new Window();

        static void Main()
        {
            #region Color Changing
            //Necessary to make the colored text work
            var handle = GetStdHandle(-11);
            GetConsoleMode(handle, out int mode);
            SetConsoleMode(handle, mode | 0x4);
            #endregion

            //Startup loop to select a new game or load an old save
            while (!initialized)
            {
                switch (Utils.AskQuestion("new or load?"))
                {
                    //1st case "New", start a new game
                    case "new":
                        Initialize();
                        break;
                    //2nd case "Load", load a prexisting file, can also type the file name to load it faster
                    case string load when load.StartsWith("load"):
                        if (AttemptLoad((load.Length > 5) ? load.Substring(5) : ""))
                        {
                            initialized = true;
                        }
                        break;
                    //3rd case 'Quit", quits the game
                    case "quit":
                        initialized = true;
                        running = false;
                        break;
                }
            }

            //Main gameplay loop
            while (running)
            {
                //Combat loop; if the combat window is open loop the attack functions
                while (!combatWindow.WindowHidden)
                {
                    combatWindow.Run();
                }

                ParseInput(Utils.GetInput());
            }

            //when game is over Close the combat Window
            combatWindow.Close();
        }

        /// <summary>
        /// Startup a new game
        /// </summary>
        static void Initialize()
        {
            player.AddItemToInventory(new InventoryItem(World.ItemByID((int)ItemIDs.STICK), 1));
            player.AddItemToInventory(new InventoryItem(World.ItemByID((int)ItemIDs.CLOTHES), 1));

            player.SetName();

            //move the player to the game's starting point
            player.MoveTo(player.home, true);

            //if player didn't quit already print out the startup text
            if (running)
                Utils.Print();

            initialized = true;
        }

        /// <summary>
        /// Attempt to load a save file
        /// </summary>
        /// <param name="fileToAttmept">if the player gave a preemptive file to load pass it through</param>
        /// <returns>returns true if a file was succefully loaded</returns>
        static bool AttemptLoad(string fileToAttmept)
        {
            string[] files;
            //Preempt by adding a file the player may be trying to load immediately
            string input = fileToAttmept;

            while (true)
            {
                files = Directory.GetFiles(@".\", "*.save");

                if (files.Length == 0)
                {
                    Utils.Add("No save files found");
                    Utils.Print();
                    return false;
                }

                //If the player didn't give a file name to load initially
                if (input == "")
                {
                    for (int x = 0; x < files.Length; x++)
                    {
                        Utils.Add("\t" + files[x].Substring(2).Trim());
                    }
                    Utils.Print();

                    input = Utils.AskQuestion("which save do you wish to load?");

                    //check for other commands the player might've given
                    switch (input)
                    {
                        //1st case "quit", quit the game
                        case "quit":
                            running = false;
                            return true;
                        //2nd case "back", return to what the player was doing initially
                        case "back":
                            return false;
                        //3rd case "help", tell the player what other commands they can use
                        case "help":
                            input = "";
                            Utils.Add("back to return to game, quit to leave");
                            Utils.Print();
                            continue;
                        //4th case "Delete", deletes the specified save file
                        case string file when file.StartsWith("delete "):
                            file = file.Substring(7);
                            for (int x = 0; x < files.Length; x++)
                            {
                                if (file == files[x].Substring(2).Trim().ToLower() || file == files[x].Substring(2).Trim().ToLower().Split('.')[0])
                                {
                                    File.Delete(files[x]);
                                    Utils.Add("save successfully deleted");
                                    break;
                                }
                            }
                            input = "";
                            Utils.Print();
                            continue;
                    }
                }

                //Check to see if there is a Save file with the name given by the player
                for (int x = 0; x < files.Length; x++)
                {
                    if (input == files[x].Substring(2).Trim().ToLower() || input == files[x].Substring(2).Trim().ToLower().Split('.')[0])
                    {
                        //there is a file with said name load it
                        Player.Load(files[x].Substring(2).Split('.')[0]);
                        Utils.Add("save successfully loaded");
                        Utils.Print();
                        return true;
                    }
                }

                //there isn't a file of said name inform player and restart loop
                Utils.Add("no save file of that name found");
                input = "";
                Utils.Print();
            }
        }

        /////////////////////////////////Parse Input///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Parse the players input to read their commands
        /// </summary>
        /// <param name="input">players input</param>
        static void ParseInput(string input)
        {
            switch (input)
            {
                //1st case "help", give the player help
                case string help when help.StartsWith("help"):
                    //if the player gave an advanced help command
                    if (help.Length > 5)
                    {
                        switch (help.Substring(5))
                        {
                            case "color":
                                World.HelpColor();
                                break;
                            case "combat":
                                World.HelpCombat();
                                break;
                            //Overflow
                            default:
                                World.Help();
                                break;
                        }
                        break;
                    }
                    World.Help();
                    break;
                //2nd case "who am i", tell the player their name
                case "who am i":
                    //If the player gave a proper name show it
                    if (player.Name.FirstName != "")
                    {
                        Utils.Add(player.Name.FullName);
                        break;
                    }

                    //otherwise ask the player to give one
                    input = Utils.AskQuestion("You did not give us your name would you like to? Yes or No");

                    if (input == "yes")
                    {
                        player.SetName();
                        break;
                    }
                    //otherwise give a passing line
                    Utils.Add("Uh, alright then");
                    break;
                //3rd case "look", allow the player to look at things
                case string look when look.StartsWith("look "):
                    player.Look(look.Substring(5).Trim());
                    break;
                //4th case "stats", show the player their stats
                case "stats":
                    player.Stats();
                    break;
                //5th case "inventory" "i", show the player their inventory
                case "inventory":
                case "i":
                    player.InventoryCheck();
                    break;
                //6th case "quest" "q", show the player their active quests
                case "quests":
                case "q":
                    Utils.Add("Current Quests: ");
                    foreach (Quest q in player.activeQuests)
                    {
                        Utils.Add("\t" + q.name);
                    }
                    break;
                //7th case "move", attempt to move the player in a specified direction
                case string move when move.StartsWith("move "):
                    player.Move(move.Substring(5).Trim());
                    break;
                //8th case "save", save the player's current progress with specified name
                case string save when save.StartsWith("save "):
                    player.Save(save.Substring(5).Trim());
                    break;
                //9th case "load", attempt to load a file, can possibly give a specified name
                case string load when load.StartsWith("load"):
                    AttemptLoad((load.Length > 5) ? load.Substring(5) : "");
                    break;
                //10th case "equip", attempt to equip a specified item
                case string equip when equip.StartsWith("equip "):
                    player.EquipItem(equip.Substring(6).Trim());
                    break;
                //11th case "Use", attempt to use a specified item
                case string use when use.StartsWith("use "):
                    player.Use(use.Substring(4));
                    break;
                //12th case "attack", attempt to attack an enemy in the Current Location
                case "attack":
                    player.Attack(player.currentLocation.monsterLivingHere);
                    break;
                //13th case "talk", attempt to talk to an NPC in the Current Location
                case "talk":
                    if (player.currentLocation.npcLivingHere != null)
                    {
                        player.currentLocation.npcLivingHere.Talk();
                        break;
                    }

                    Utils.Add("You talk to the air");
                    break;
                //14th case "quit", quits the game
                case "quit":
                    running = false;
                    break;
                //Overflow
                default:
                    Utils.Add("I- I don- I don't understand, type 'help' for commands");
                    break;
            }

            //print the pent up string
            Utils.Print();
        }
    }
}