using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CRPGNamespace
{
    public class Player : LivingCreature
    {
        public Weapon currentWeapon;
        public Armor currentArmor;
        public Location currentLocation;
        public Location home;
        public List<InventoryItem> Inventory = new List<InventoryItem>();
        public List<Quest> activeQuests = new List<Quest>();
        public List<Quest> completedQuests = new List<Quest>();
        public int gold = 0;
        public int XP = 0;

        int XPToLevelUp = 0;
        int level = 1;
        int baseMaxDamage = 1;
        int baseMinDamage = 0;
        int baseAc = 0;

        #region current stat vals
        public int Level
        {
            get => level;
        }
        public int CurrentAc
        {
            get
            {
                return baseAc + (currentArmor != null ? currentArmor.ac : 0);
            }
        }
        public int CurrentMaxDamage
        {
            get
            {
                return baseMaxDamage + (currentWeapon != null ? currentWeapon.WeaponAttack.maxDamage : 0);
            }
        }
        public int CurrentMinDamage
        {
            get
            {
                return baseMinDamage + (currentWeapon != null ? currentWeapon.WeaponAttack.minDamage : 0);
            }
        }
        #endregion

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

        public Player(string[] saveData)
        {
            //name, gold, xp, xptolevelup, level, maximumHP, currentWeaponID, currentArmorID, homeID, Inventory, quests, clearedLocs
            Name = new Name(saveData[0], saveData[1], saveData[2]);
            int.TryParse(saveData[3], out gold);
            int.TryParse(saveData[4], out XP);
            int.TryParse(saveData[5], out XPToLevelUp);
            int.TryParse(saveData[6], out level);
            int.TryParse(saveData[7], out maximumHP);

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

            int.TryParse(saveData[11], out tempInt);
            int[] tmpBytes = ParseIDs(tempInt);
            for(int x = 0; x < tmpBytes.Length; x++)
            {
                int.TryParse(saveData[12 + x], out tempInt);
                Inventory.Add(new InventoryItem(World.ItemByID(tmpBytes[x]), tempInt));
            }
            int buffer = tmpBytes.Length;
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
            int.TryParse(saveData[13 + buffer], out tempInt);
            tmpBytes = ParseIDs(tempInt);
            for (int x = 0; x < tmpBytes.Length; x++)
            {
                Quest tmpQuest = World.QuestByID(tmpBytes[x]);
                completedQuests.Add(tmpQuest);
                tmpQuest.complete = true;
            }
            int.TryParse(saveData[14 + buffer], out tempInt);
            tmpBytes = ParseIDs(tempInt);
            for (int x = 0; x < tmpBytes.Length; x++)
            {
                World.LocationByID(tmpBytes[x]).monsterLivingHere = null;
            }

            MoveTo(home, true);
        }

        public void SetHome()
        {
            home = currentLocation;
        }

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

        public void EarnXP(int XPEarned)
        {
            XP += XPEarned;

            if (XP >= XPToLevelUp)
            {
                level++;
                XP -= XPToLevelUp;
                XPToLevelUp += (int)(XPToLevelUp * 0.3f);

                maximumHP += (int)((float)maximumHP * ((float)level / 4f));
                currentHP = maximumHP;
                baseMaxDamage = level + (level - 1); 
                baseMinDamage = level / 2;
                baseAc = (2 * level) - 2;
            }
        }

        public void GainQuest(Quest quest)
        {
            if (quest.playerHasQuest || quest.complete)
            {
                return;
            }

            Utils.Add(Utils.ColorText(quest.questGainedText, TextColor.MAGENTA));
            activeQuests.Add(quest);
            quest.playerHasQuest = true;
        }

        #region save and load
        public void Save(string saveName)
        {
            if(File.Exists(saveName + ".save"))
            {
                File.Delete(saveName + ".save");
            }

            int invTypes = 0;
            int[] invQuants = new int[Inventory.Count];
            for (int x = 0; x < Inventory.Count; x++)
            {
                invTypes += Inventory[x].details.ID;
                invQuants[x] = (Inventory[x].quantity);
            }
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
            int compQuests = 0;
            for(int x = 0; x < completedQuests.Count; x++)
            {
                compQuests += completedQuests[x].ID;
            }
            int clearedLocs = 0;
            for (int x = 0; x < World.Locations.Count; x++)
            {
                if(World.Locations[x].monsterLivingHere == null)
                {
                    clearedLocs += World.Locations[x].ID;
                }
            }

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
        public static int[] ParseIDs(int IDs)
        {
            byte[] bytes = BitConverter.GetBytes(IDs);
            BitArray bitArray = new BitArray(bytes);
            List<int> ints = new List<int>();
            for (int x = 0; x < bitArray.Count; x++)
            {
                if (((bitArray[x]) ? 1 : 0) << x != 0)
                {
                    ints.Add(1 << x);
                }
            }
            return ints.ToArray();
        }

        public static void Load(string saveName)
        {
            World.Reload();
            string saveText = File.ReadAllText(saveName + ".save");
            Program.player = new Player(saveText.Split(","));
        }
        #endregion

        #region Location moving
        public void MoveTo(Location loc, bool ignoreMonster = false)
        {
            if(!ignoreMonster && currentLocation != null && currentLocation.monsterLivingHere != null)
            {
                Utils.Add($"The {currentLocation.monsterLivingHere.Name.FullName} blocks your path");
                return;
            }

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

        public void Move(string arg)
        {
            switch (arg)
            {
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
                default:
                    Utils.Add("that's not a direction");
                    break;
            }
        }
        #endregion

        #region Item gaining and equiping
        public void Use(string arg)
        {
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

        public void AddItemToInventory(InventoryItem itemToAdd)
        {
            if(itemToAdd.details is QuestItem)
            {
                (itemToAdd.details as QuestItem).CallQuest();
            }

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
        public void RemoveItemFromInventory(InventoryItem itemToRemove)
        {
            InventoryItem tmpItem = Inventory.Find(s => s == itemToRemove);
            if (tmpItem.quantity > 1)
            {
                tmpItem.quantity--;
                Inventory[Inventory.FindIndex(s => s == tmpItem)] = tmpItem;
                return;
            }

            Inventory.Remove(tmpItem);
        }

        public void EquipItem(string arg)
        {
            foreach(InventoryItem item in Inventory)
            {
                if(item.details.Name.ToLower() == arg)
                {
                    Item tmpItem = item.details;

                    if(tmpItem is Weapon weapon)
                    {
                        currentWeapon = weapon;
                        Utils.Add("Equipped " + weapon.Name);
                        return;
                    }

                    if (item.details is Armor armor)
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
        public override void TakeDamage()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
