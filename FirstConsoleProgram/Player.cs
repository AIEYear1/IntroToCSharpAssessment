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
        public int gold = 0;
        public int XP = 0;
        int XPToLevelUp = 0;
        int level = 1;
        int baseMaxDamage = 1;
        int baseMinDamage = 0;
        int baseAc = 0;
        public Weapon currentWeapon;
        public Armor currentArmor;
        public Location currentLocation;
        public Location home;
        public List<InventoryItem> Inventory = new List<InventoryItem>();
        public List<Quest> activeQuests = new List<Quest>();

        #region current stat vals
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
            name = new Name(saveData[0], saveData[1], saveData[2]);
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
            int[] tmpBytes = World.ParseIDs(tempInt);
            for(int x = 0; x < tmpBytes.Length; x++)
            {
                int.TryParse(saveData[12 + x], out tempInt);
                Inventory.Add(new InventoryItem(World.ItemByID(tmpBytes[x]), tempInt));
            }
            int buffer = tmpBytes.Length;
            int.TryParse(saveData[12 + buffer], out tempInt);
            tmpBytes = World.ParseIDs(tempInt);
            for (int x = 0; x < tmpBytes.Length; x++)
            {
                int.TryParse(saveData[13 + buffer + x], out tempInt);
                Quest tmpQuest = World.QuestByID(tmpBytes[x]);
                activeQuests.Add(tmpQuest);
                tmpQuest.ObjectiveMarker(tempInt);
            }
            buffer += tmpBytes.Length;
            int.TryParse(saveData[13 + buffer], out tempInt);
            tmpBytes = World.ParseIDs(tempInt);
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
            name = new Name((nameDets.Length >= 1) ? nameDets[0] : "", (nameDets.Length >= 3) ? nameDets[2] : (nameDets.Length >= 2) ? nameDets[1] : "", (nameDets.Length >= 3) ? nameDets[1] : "");

            Console.WriteLine("Welcome " + name.FirstName);
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
            int clearedLocs = 0;
            for (int x = 0; x < World.Locations.Count; x++)
            {
                if(World.Locations[x].monsterLivingHere == null)
                {
                    clearedLocs += World.Locations[x].ID;
                }
            }

            string saveText = $"{name.FirstName},{name.MiddleName},{name.LastName}";
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
            saveText += "," + clearedLocs;

            File.AppendAllText(saveName + ".save", saveText);

            Utils.Add("save successful");
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
                Utils.Add($"The {currentLocation.monsterLivingHere.name.FullName} blocks your path");
                return;
            }

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

        public void MoveNorth()
        {
            if (currentLocation.locationToNorth != null)
            {
                MoveTo(currentLocation.locationToNorth);
            }
            else
            {
                Utils.Add("you cannot move north");
            }
        }
        public void MoveEast()
        {
            if (currentLocation.locationToEast != null)
            {
                MoveTo(currentLocation.locationToEast);
            }
            else
            {
                Utils.Add("you cannot move east");
            }
        }
        public void MoveSouth()
        {
            if (currentLocation.locationToSouth != null)
            {
                MoveTo(currentLocation.locationToSouth);
            }
            else
            {
                Utils.Add("you cannot move south");
            }
        }
        public void MoveWest()
        {
            if (currentLocation.locationToWest != null)
            {
                MoveTo(currentLocation.locationToWest);
            }
            else
            {
                Utils.Add("you cannot move West");
            }
        }
        #endregion

        #region Item gaining and equiping
        public void AddItemToInventory(InventoryItem itemToAdd)
        {
            if(itemToAdd.details is QuestItem)
            {
                (itemToAdd.details as QuestItem).CallQuest();
            }

            foreach(InventoryItem item in Inventory)
            {
                if(itemToAdd.details == item.details)
                {
                    item.quantity++;
                    return;
                }
            }

            Inventory.Add(itemToAdd);
        }
        public void RemoveItemFromInventory(InventoryItem itemToRemove)
        {
            if (itemToRemove.details is QuestItem)
            {
                Utils.Add("Can't sell quest items");
            }
            if (!Inventory.Contains(itemToRemove))
            {
                Utils.Add("You don't have this item");
                return;
            }

            if(itemToRemove.quantity > 1)
            {
                itemToRemove.quantity--;
                return;
            }

            Inventory.Remove(itemToRemove);
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
                case "north":
                case "up":
                case "n":
                    currentLocation.LookNorth();
                    break;
                case "east":
                case "right":
                case "e":
                    currentLocation.LookEast();
                    break;
                case "south":
                case "down":
                case "s":
                    currentLocation.LookSouth();
                    break;
                case "west":
                case "left":
                case "w":
                    currentLocation.LookWest();
                    break;
                case "here":
                    currentLocation.LookHere();
                    break;
                case string item when Inventory.SingleOrDefault(x => x.details.Name.ToLower() == item || x.details.NamePlural.ToLower() == item) != null:
                    Inventory.SingleOrDefault(x => x.details.Name.ToLower() == item || x.details.NamePlural.ToLower() == item).details.Look();
                    break;
                case string monster when currentLocation.monsterLivingHere != null && currentLocation.monsterLivingHere.name.FullName.ToLower() == monster:
                    currentLocation.monsterLivingHere.LookAt();
                    break;
                case string quest when activeQuests.SingleOrDefault(x => x.name.ToLower() == quest) != null:
                    activeQuests.SingleOrDefault(x => x.name.ToLower() == quest).LookQuest();
                    break;
                default:
                    Utils.Add("Please specify what to look at");
                    break;
            }
        }

        public void Stats()
        {
            Utils.Add($"Stats for {name.FullName}");
            Utils.Add($"\tHP: \t\t{currentHP}/{maximumHP}");

            if(currentWeapon != null)
                Utils.Add($"\tAttack Power: \t{CurrentMinDamage}-{CurrentMaxDamage}");
            if (currentArmor != null)
                Utils.Add($"\tProtection: \t{CurrentAc}");

            Utils.Add($"\tLevel: \t\t{level}");
            Utils.Add($"\tXP: \t\t{XP}/{XPToLevelUp}");
            Utils.Add($"\tGold: \t\t{gold}");
        }

        #endregion

        public void GainQuest(Quest quest)
        {
            if(quest.playerHasQuest || quest.complete)
            {
                return;
            }

            Utils.Add(Utils.ColorText(quest.questGainedText, TextColor.MAGENTA));
            activeQuests.Add(quest);
            quest.playerHasQuest = true;
        }

        public override void TakeDamage(int damage)
        {
            damage = (int)MathF.Max(1, damage - CurrentAc);
            currentHP -= damage;
            Utils.Add($"You took {Utils.ColorText(damage.ToString(), TextColor.BLUE)} damage!");
            if (currentHP <= 0)
            {
                Utils.Add(Utils.ColorText(name.FullName + " has died!", TextColor.DARKRED));
                MoveTo(home, true);
                Program.combatWindow.EndAttack();
            }
        }

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

            enemToAttack.knownNoun = true;

            Program.combatWindow.StartAttack(this, enemToAttack);
        }
    }
}
