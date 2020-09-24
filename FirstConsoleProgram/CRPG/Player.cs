using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CRPGNamespace
{
    /// <summary>
    /// The player of the game, handles a number of the functions
    /// </summary>
    public class Player : LivingCreature
    {
        /// <summary>
        /// Player's current weapon
        /// </summary>
        public Weapon currentWeapon;
        /// <summary>
        /// Player's current armor
        /// </summary>
        public Armor currentArmor;
        /// <summary>
        /// Player's current loccation
        /// </summary>
        public Location currentLocation;
        /// <summary>
        /// Where the player will return to should they die or load a save
        /// </summary>
        public Location home;
        /// <summary>
        /// Player's current inventory
        /// </summary>
        public List<InventoryItem> Inventory = new List<InventoryItem>();
        /// <summary>
        /// Player's active quests
        /// </summary>
        public List<Quest> activeQuests = new List<Quest>();
        /// <summary>
        /// Player's completed quests
        /// </summary>
        public List<Quest> completedQuests = new List<Quest>();
        /// <summary>
        /// Player's total gold
        /// </summary>
        public int gold = 0;
        /// <summary>
        /// Player's current XP
        /// </summary>
        public int XP = 0;

        //The amount of XP the player needs until they level up
        int XPToLevelUp = 0;
        //The player's current level
        int level = 1;
        //Base values to help make levels feel more important
        int baseMaxDamage = 1;
        int baseMinDamage = 0;
        int baseAc = 0;

        #region current stat vals
        /// <summary>
        /// Readonly version of level for referencing
        /// </summary>
        public int Level
        {
            get => level;
        }
        /// <summary>
        /// Readonly AC value of the player
        /// </summary>
        public int CurrentAc
        {
            get
            {
                return baseAc + (currentArmor != null ? currentArmor.ac : 0);
            }
        }
        /// <summary>
        /// Readonly Maximum damage value of the player
        /// </summary>
        public int CurrentMaxDamage
        {
            get
            {
                return baseMaxDamage + (currentWeapon != null ? currentWeapon.WeaponAttack.maxDamage : 0);
            }
        }
        /// <summary>
        /// Readonly Minimum damage value of the player
        /// </summary>
        public int CurrentMinDamage
        {
            get
            {
                return baseMinDamage + (currentWeapon != null ? currentWeapon.WeaponAttack.minDamage : 0);
            }
        }

        /// <summary>
        /// Writeonly value for stting player's home
        /// </summary>
        public Location Home
        {
            set => home = value;
        }
        #endregion

        /// Parameters
        /// <param name="gold">Player's start gold amount</param>
        /// <param name="xP">Player's start XP amount</param>
        /// <param name="xPToLevelUp">Player's start XP to reach next level</param>
        /// <param name="level">Player's start level</param>
        /// <param name="currentWeapon">Player's start weapon</param>
        /// <param name="currentArmor">Player's start armor</param>
        /// <param name="home">Player's starting home</param>
        /// <param name="HP">Player's start HP</param>
        public Player(int gold, int xP, int xPToLevelUp, int level, Weapon currentWeapon, Armor currentArmor, Location home, int HP, bool knownNoun = false, bool properNoun = false) : base(HP, knownNoun, properNoun)
        {
            this.gold = gold;
            XP = xP;
            XPToLevelUp = xPToLevelUp;
            this.level = level;
            this.currentWeapon = currentWeapon;
            this.currentArmor = currentArmor;
            this.home = home;
        }

        /// <summary>
        /// Player Constructor for loading a save file
        /// </summary>
        /// <param name="saveData">Data to load from</param>
        public Player(string[] saveData)
        {
            //name, gold, xp, xptolevelup, level, maximumHP, currentWeaponID, currentArmorID, homeID, Inventory, quests, clearedLocs

            //parse out simple / base data
            Name = new Name(saveData[0], saveData[1], saveData[2]);
            int.TryParse(saveData[3], out gold);
            int.TryParse(saveData[4], out XP);
            int.TryParse(saveData[5], out XPToLevelUp);
            int.TryParse(saveData[6], out level);
            int.TryParse(saveData[7], out maximumHP);

            //Sets simple data that can be mathed off of player level
            currentHP = maximumHP;
            baseMaxDamage = level + (level - 1);
            baseMinDamage = level / 2;
            baseAc = (2 * level) - 2;


            int.TryParse(saveData[8], out int tempInt);
            currentWeapon = (Weapon)World.ItemByID(tempInt);

            int.TryParse(saveData[9], out tempInt);
            currentArmor = (Armor)World.ItemByID(tempInt);

            int.TryParse(saveData[10], out tempInt);
            home = World.LocationByID(tempInt);


            //Fill out player inventory
            int.TryParse(saveData[11], out tempInt);
            int[] tmpBytes = ParseIDs(tempInt);
            for(int x = 0; x < tmpBytes.Length; x++)
            {
                int.TryParse(saveData[12 + x], out tempInt);
                Inventory.Add(new InventoryItem(World.ItemByID(tmpBytes[x]), tempInt));
            }

            ///Buffer accounts for the separation caused by values which fill out an abstract number of csv lines
            int buffer = tmpBytes.Length;

            //Add all of the active quests and set their objectives
            int.TryParse(saveData[12 + buffer], out tempInt);
            tmpBytes = ParseIDs(tempInt);
            for (int x = 0; x < tmpBytes.Length; x++)
            {
                int.TryParse(saveData[13 + buffer + x], out tempInt);
                Quest tmpQuest = World.QuestByID(tmpBytes[x]);
                activeQuests.Add(tmpQuest);
                tmpQuest.ObjectiveMarker(tempInt);
            }

            buffer += tmpBytes.Length;

            //Adds all of the quests the player has completed
            int.TryParse(saveData[13 + buffer], out tempInt);
            tmpBytes = ParseIDs(tempInt);
            for (int x = 0; x < tmpBytes.Length; x++)
            {
                Quest tmpQuest = World.QuestByID(tmpBytes[x]);
                completedQuests.Add(tmpQuest);
                tmpQuest.complete = true;
            }

            //Sets all of the cleared locations to not have any monsters
            int.TryParse(saveData[14 + buffer], out tempInt);
            tmpBytes = ParseIDs(tempInt);
            for (int x = 0; x < tmpBytes.Length; x++)
            {
                World.LocationByID(tmpBytes[x]).monsterLivingHere = null;
            }

            MoveTo(home, true);
        }

        /// <summary>
        /// Set's the player's name
        /// </summary>
        public void SetName()
        {
            string input = Utils.AskQuestion("What is your name");

            if (input == "quit")
            {
                Program.running = false;
                return;
            }

            string[] nameDets = input.Split(' ');
            Name = new Name((nameDets.Length >= 1) ? nameDets[0] : "", (nameDets.Length >= 3) ? nameDets[2] : (nameDets.Length >= 2) ? nameDets[1] : "", (nameDets.Length >= 3) ? nameDets[1] : "");

            Utils.Add(Utils.ColorText($"Welcome {Name.FirstName}! type 'help' for commands", TextColor.LIME));
        }

        /// <summary>
        /// Gives the player XP and checks for level up
        /// </summary>
        /// <param name="XPEarned">XP to give the player</param>
        public void EarnXP(int XPEarned)
        {
            XP += XPEarned;

            //If the player shouldn't level up
            if (XP < XPToLevelUp)
                return;

            //if the player should level up
            level++;
            XP -= XPToLevelUp;
            XPToLevelUp += (int)(XPToLevelUp * 0.3f);

            maximumHP += (int)((float)maximumHP * ((float)level / 4f));
            currentHP = maximumHP;
            baseMaxDamage = level + (level - 1); 
            baseMinDamage = level / 2;
            baseAc = (2 * level) - 2;
        }

        /// <summary>
        /// Gives the player a new quest
        /// </summary>
        /// <param name="quest">quest to give player</param>
        public void GainQuest(Quest quest)
        {
            //If the player already has the quest or has already completed the quest, return
            if (quest.playerHasQuest || quest.complete)
                return;

            Utils.Add(Utils.ColorText(quest.questGainedText, TextColor.MAGENTA));
            activeQuests.Add(quest);
            quest.playerHasQuest = true;
        }

        #region save and load
        /// <summary>
        /// Saves the game as a csv
        /// </summary>
        /// <param name="saveName">Name of the file to save as</param>
        public void Save(string saveName)
        {
            // overwrites old saves
            if(File.Exists(saveName + ".save"))
            {
                File.Delete(saveName + ".save");
            }

            //converts player inventory into data
            int invTypes = 0;
            int[] invQuants = new int[Inventory.Count];
            for (int x = 0; x < Inventory.Count; x++)
            {
                invTypes += Inventory[x].details.ID;
                invQuants[x] = (Inventory[x].quantity);
            }

            //converts player's active quests into data
            int quests = 0;
            int[] questProgress = new int[activeQuests.Count];
            for (int x = 0; x < activeQuests.Count; x++)
            {
                quests += activeQuests[x].ID;
                for(int y = 0; y < activeQuests[x].objectives.Count; y++)
                {
                    if (!activeQuests[x].objectives[y].Complete)
                    {
                        questProgress[x] = y-1;
                        break;
                    }
                }
            }
            //converts player's completed quests into data
            int compQuests = 0;
            for(int x = 0; x < completedQuests.Count; x++)
            {
                compQuests += completedQuests[x].ID;
            }
            //Marks all locations that don't have enemies so after loading the locations still won't have enemies
            int clearedLocs = 0;
            for (int x = 0; x < World.Locations.Count; x++)
            {
                if(World.Locations[x].monsterLivingHere == null)
                {
                    clearedLocs += World.Locations[x].ID;
                }
            }

            //Takes all the data and manually converts it into a csv
            string saveText = $"{Name.FirstName},{Name.MiddleName},{Name.LastName}";
            saveText += "," + gold;
            saveText += "," + XP;
            saveText += "," + XPToLevelUp;
            saveText += "," + level;
            saveText += "," + maximumHP;
            saveText += "," + currentWeapon.ID;
            saveText += "," + currentArmor.ID;
            saveText += "," + home.ID;
            saveText += $",{invTypes},{Utils.ToString(invQuants, ",")}";
            saveText += $",{quests},{Utils.ToString(questProgress, ",")}";
            saveText += "," + compQuests;
            saveText += "," + clearedLocs;

            File.AppendAllText(saveName + ".save", saveText);

            Utils.Add("save successful");
        }

        /// <summary>
        /// convert int into binary array
        /// </summary>
        /// <param name="IDs">int which contains all of the bits</param>
        /// <returns>returns an array of only the 1s from a binary array</returns>
        public static int[] ParseIDs(int IDs)
        {
            List<int> toReturn = new List<int>();
            byte[] bytes = BitConverter.GetBytes(IDs);
            BitArray bitArray = new BitArray(bytes);
            for (int x = 0; x < bitArray.Count; x++)
            {
                if (((bitArray[x]) ? 1 : 0) << x != 0)
                {
                    toReturn.Add(1 << x);
                }
            }
            return toReturn.ToArray();
        }

        /// <summary>
        /// Reloads the world and player from a save file
        /// </summary>
        /// <param name="saveName">name of save file to load from</param>
        public static void Load(string saveName)
        {
            World.Reload();
            string saveText = File.ReadAllText(saveName + ".save");
            Program.player = new Player(saveText.Split(","));
        }
        #endregion

        #region Location moving
        /// <summary>
        /// Move to the specified location
        /// </summary>
        /// <param name="loc">Location to move to</param>
        /// <param name="ignoreMonster">Whether to ignore the monster when moving (for loading and dieing)</param>
        public void MoveTo(Location loc, bool ignoreMonster = false)
        {
            //if there is a monster in your current location and you are not ignoring it get blocked
            if(!ignoreMonster && currentLocation != null && currentLocation.monsterLivingHere != null)
            {
                Utils.Add($"The {currentLocation.monsterLivingHere.Name.FullName} blocks your path");
                return;
            }

            //If the location is locked and the player doesn't meet the requirements to enter
            if ((loc is LockedLocation lockLoc) && !lockLoc.Enter())
                return;

            if(loc.monsterLivingHere != null)
            {
                loc.monsterLivingHere.currentHP = loc.monsterLivingHere.maximumHP;
            }

            currentHP = maximumHP;
            currentLocation = loc;
            currentLocation.knownNoun = true;

            if(loc is QuestLocation)
            {
                (loc as QuestLocation).CallQuest();
            }

            currentLocation.LookHere();
        }

        /// <summary>
        /// For player command movement checks all directional locations and attempts to move in that direction if there is a location there
        /// </summary>
        /// <param name="dir">direction to travel in</param>
        public void Move(string dir)
        {
            switch (dir)
            {
                //1st case "North", checks the north direction
                case "north":
                case "up":
                case "n":
                    if (currentLocation.locationToNorth != null)
                    {
                        MoveTo(currentLocation.locationToNorth);
                        return;
                    }
                    Utils.Add("you cannot move North");
                    return;
                //2nd case "East", checks the east direction
                case "east":
                case "right":
                case "e":
                    if (currentLocation.locationToEast != null)
                    {
                        MoveTo(currentLocation.locationToEast);
                        return;
                    }
                    Utils.Add("you cannot move East");
                    return;
                //3rd case "South", checks the south direction
                case "south":
                case "down":
                case "s":
                    if (currentLocation.locationToSouth != null)
                    {
                        MoveTo(currentLocation.locationToSouth);
                        return;
                    }
                    Utils.Add("you cannot move South");
                    return;
                //4th case "West", checks the west direction
                case "west":
                case "left":
                case "w":
                    if (currentLocation.locationToWest != null)
                    {
                        MoveTo(currentLocation.locationToWest);
                        return;
                    }
                    Utils.Add("you cannot move West");
                    return;
                //Overflow
                default:
                    Utils.Add("that's not a direction");
                    break;
            }
        }
        #endregion

        #region Item gaining and equiping
        /// <summary>
        /// Attempts to use an item
        /// </summary>
        /// <param name="arg">name of the item the player is trying to use</param>
        public void Use(string arg)
        {
            //Checks to see if the player has the item the player is asking for
            if(Inventory.SingleOrDefault(item => item.details.Name == arg || item.details.NamePlural == arg) == InventoryItem.Empty)
            {
                Utils.Add("You don't have an item of that name");
                return;
            }

            Item tmpItem = Inventory.SingleOrDefault(item => item.details.Name == arg || item.details.NamePlural == arg).details;
            if(tmpItem is Consumable consumable)
            {
                consumable.Consume(Program.player);
                return;
            }

            Utils.Add("You can't use this item");
        }

        /// <summary>
        /// Adds an item to the player's inventory
        /// </summary>
        /// <param name="itemToAdd">Item to add</param>
        public void AddItemToInventory(InventoryItem itemToAdd)
        {
            if(itemToAdd.details is QuestItem)
            {
                (itemToAdd.details as QuestItem).CallQuest();
            }

            //If the player already has this item increase the quantity
            for(int x = 0; x<Inventory.Count; x++)
            {
                if (itemToAdd == Inventory[x])
                {
                    InventoryItem tmpItem = Inventory[x];
                    tmpItem.quantity++;
                    Inventory[x] = tmpItem;
                    return;
                }
            }

            Inventory.Add(itemToAdd);
        }
        /// <summary>
        /// Removes an item from the player's inventory
        /// </summary>
        /// <param name="itemToRemove">Item to remove</param>
        public void RemoveItemFromInventory(InventoryItem itemToRemove)
        {
            InventoryItem tmpItem = Inventory.Find(s => s == itemToRemove);

            //if the player has more than one of said item decrease the quantity
            if (tmpItem.quantity > 1)
            {
                tmpItem.quantity--;
                Inventory[Inventory.FindIndex(s => s == tmpItem)] = tmpItem;
                return;
            }

            Inventory.Remove(tmpItem);
        }

        /// <summary>
        /// Attempts to equip and item
        /// </summary>
        /// <param name="arg">Item to equip</param>
        public void EquipItem(string arg)
        {
            for(int x = 0; x <Inventory.Count; x++)
            {
                if (Inventory[x].details.Name.ToLower() == arg)
                {
                    Item tmpItem = Inventory[x].details;

                    if (tmpItem is Weapon weapon)
                    {
                        currentWeapon = weapon;
                        Utils.Add("Equipped " + weapon.Name);
                        return;
                    }

                    if (tmpItem is Armor armor)
                    {
                        currentArmor = armor;
                        Utils.Add("Equipped " + armor.Name);
                        return;
                    }

                    Utils.Add("You can't equip " + tmpItem.Name);
                    return;
                }
            }

            Utils.Add("You have no items called " + arg);
        }
        #endregion

        #region Looking and info
        /// <summary>
        /// Look Command manager
        /// </summary>
        /// <param name="arg"></param>
        public void Look(string arg)
        {
            switch (arg)
            {
                //1st case Location North
                case "north":
                case "up":
                case "n":
                    currentLocation.LookDirection("North");
                    return;
                //2nd case Location East
                case "east":
                case "right":
                case "e":
                    currentLocation.LookDirection("East");
                    return;
                //3rd case Location South 
                case "south":
                case "down":
                case "s":
                    currentLocation.LookDirection("South");
                    return;
                //4th case Location West
                case "west":
                case "left":
                case "w":
                    currentLocation.LookDirection("West");
                    return;
                //5th case Current Location
                case "here":
                    currentLocation.LookHere();
                    return;
                //6th case an item in the inventory
                case string item when Inventory.SingleOrDefault(x => x.details.Name.ToLower() == item || x.details.NamePlural.ToLower() == item) != InventoryItem.Empty:
                    Inventory.SingleOrDefault(x => x.details.Name.ToLower() == item || x.details.NamePlural.ToLower() == item).details.Look();
                    return;
                //7th case Current Monster
                case string monster when currentLocation.monsterLivingHere != null && currentLocation.monsterLivingHere.Name.FullName.ToLower() == monster:
                    currentLocation.monsterLivingHere.LookAt();
                    return;
                //7th case Current NPC
                case string npc when currentLocation.npcLivingHere != null && currentLocation.npcLivingHere.name.FullName.ToLower() == npc:
                    currentLocation.npcLivingHere.Look();
                    return;
                //8th case a quest the player has
                case string quest when activeQuests.SingleOrDefault(x => x.name.ToLower() == quest) != null:
                    activeQuests.SingleOrDefault(x => x.name.ToLower() == quest).LookQuest();
                    return;
                //overflow
                default:
                    Utils.Add("Please specify what to look at");
                    return;
            }
        }

        /// <summary>
        /// shows player's current stats
        /// </summary>
        public void Stats()
        {
            Utils.Add($"Stats for {Name.FullName}");
            Utils.Add($"\tHP: \t\t{currentHP}/{maximumHP}");

            if(currentWeapon != null)
                Utils.Add($"\tAttack Power: \t{CurrentMinDamage}-{CurrentMaxDamage}");
            if (currentArmor != null)
                Utils.Add($"\tProtection: \t{CurrentAc}");

            Utils.Add($"\tLevel: \t\t{level}");
            Utils.Add($"\tXP: \t\t{XP}/{XPToLevelUp}");
            Utils.Add($"\tGold: \t\t{gold}");
        }

        /// <summary>
        /// Shows the player's inventory
        /// </summary>
        public void InventoryCheck()
        {
            Utils.Add("Current Inventory: ");
            for (int x = 0; x < Inventory.Count; x++)
            {
                if (Inventory[x].details is Weapon)
                {
                    Utils.Add($"\t{Utils.ColorText(Inventory[x].details.Name, TextColor.SALMON)} : {Inventory[x].quantity}");
                    continue;
                }
                if (Inventory[x].details is Armor)
                {
                    Utils.Add($"\t{Utils.ColorText(Inventory[x].details.Name, TextColor.LIGHTBLUE)} : {Inventory[x].quantity}");
                    continue;
                }
                if (Inventory[x].details is Consumable)
                {
                    Utils.Add($"\t{Utils.ColorText(Inventory[x].details.Name, TextColor.PINK)} : {Inventory[x].quantity}");
                    continue;
                }

                Utils.Add($"\t{Utils.ColorText(Inventory[x].details.Name, TextColor.GOLD)} : {Inventory[x].quantity}");
            }
        }
        #endregion

        #region Combat
        /// <summary>
        /// Begins the attack sequence
        /// </summary>
        /// <param name="enemToAttack">Enemy to attack</param>
        public void Attack(Monster enemToAttack)
        {
            if (currentWeapon == null)
            {
                Utils.Add("You need a weapon to attack");
                return;
            }
            if (enemToAttack == null)
            {
                Utils.Add("You swing wildly at the air");
                return;
            }

            enemToAttack.KnownNoun = true;

            Program.combatWindow.StartAttack(this, enemToAttack);
        }

        /// <summary>
        /// Take damage from the enemy attack
        /// </summary>
        /// <param name="damage">damage to take</param>
        public override void TakeDamage(int damage)
        {
            damage = (int)MathF.Max(1, damage - CurrentAc);
            currentHP -= damage;
            Utils.Add($"You took {Utils.ColorText(damage.ToString(), TextColor.BLUE)} damage!");
            if (currentHP <= 0)
            {
                Utils.Add(Utils.ColorText(Name.FullName + " has died!", TextColor.DARKRED));
                MoveTo(home, true);
                Program.combatWindow.EndAttack();
            }
        }

        /// <summary>
        /// Override for abstract class NOT IMPLEMENTED
        /// </summary>
        public override int TakeDamage()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
