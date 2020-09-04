using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CRPGThing
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
        public readonly Location home;
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
                return baseMaxDamage + (currentWeapon != null ? currentWeapon.weaponAttack.maxDamage : 0);
            }
        }
        public int CurrentMinDamage
        {
            get
            {
                return baseMinDamage + (currentWeapon != null ? currentWeapon.weaponAttack.minDamage : 0);
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
                XPToLevelUp = XPToLevelUp + (int)(XPToLevelUp * 0.3f);

                maximumHP += (int)((float)maximumHP * ((float)level / 4f));
                currentHP = maximumHP;
                baseMaxDamage = level + (level - 1); 
                baseMinDamage = level / 2;
                baseAc = (2 * level) - 2;
            }
        }

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
                if(item.details.name.ToLower() == arg)
                {
                    Item tmpItem = item.details;

                    if(tmpItem is Weapon)
                    {
                        currentWeapon = (Weapon)tmpItem;
                        Utils.Add("Equipped " + tmpItem.name);
                        return;
                    }

                    if (item.details is Armor)
                    {
                        currentArmor = (Armor)tmpItem;
                        Utils.Add("Equipped " + tmpItem.name);
                        return;
                    }

                    Utils.Add("You can't equip " + tmpItem.name);
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
                case string item when Inventory.SingleOrDefault(x => x.details.name.ToLower() == item || x.details.namePlural.ToLower() == item) != null:
                    Inventory.SingleOrDefault(x => x.details.name.ToLower() == item || x.details.namePlural.ToLower() == item).details.Look();
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

            int damage = Utils.NumberBetween(CurrentMinDamage, CurrentMaxDamage);
            enemToAttack.currentHP -= damage;
            Utils.Add($"You hit {Utils.PrefixNoun(enemToAttack.name.FullName, enemToAttack.properNoun, enemToAttack.knownNoun, TextColor.RED)} for {Utils.ColorText(damage.ToString(), TextColor.BLUE)} damage!");

            if (enemToAttack.currentHP <= 0)
            {
                enemToAttack.Die(this);

                return;
            }

            enemToAttack.Attack(this);
        }
    }
}
