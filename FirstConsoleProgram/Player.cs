using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace CRPGThing
{
    public class Player : LivingCreature
    {
        public Name name;
        public int gold = 0;
        public int XP = 0;
        public int XPToLevelUp = 0;
        public int level = 1;
        public Weapon currentWeapon;
        public Armor currentArmor;
        public Location currentLocation;
        public Location home;
        public List<InventoryItem> Inventory = new List<InventoryItem>();
        public List<Quest> activeQuests = new List<Quest>();

        public Player(Name name, int gold, int xP, int xPToLevelUp, int level, Weapon currentWeapon, Armor currentArmor, Location home, int HP) : base(HP)
        {
            this.name = name;
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

        #region Location moving

        public void MoveTo(Location loc)
        {
            currentHP = maximumHP;
            currentLocation = loc;

            currentLocation.LookHere();

            if(loc is QuestLocation)
            {
                (loc as QuestLocation).CallQuest();
            }
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
                    Item tmpItem = Inventory.SingleOrDefault(x => x.details.name.ToLower() == item || x.details.namePlural.ToLower() == item).details;
                    Utils.Add(tmpItem.name);
                    if(tmpItem is Weapon)
                    {
                        Utils.Add($"\tAttack Power: {(tmpItem as Weapon).minDamage}-{(tmpItem as Weapon).maxDamage}");
                        break;
                    }
                    if(tmpItem is Armor)
                    {
                        Utils.Add($"\tProtection Level: {(tmpItem as Armor).ac}");
                    }
                    break;
                case string monster when currentLocation.monsterLivingHere != null && currentLocation.monsterLivingHere.name.FullName.ToLower() == monster:
                    currentLocation.monsterLivingHere.LookAt();
                    break;
                default:
                    Utils.Add("Please specify what to look at");
                    break;
            }
        }

        public void Stats()
        {
            Utils.Add($"\nStats for {name.FullName}");
            Utils.Add($"\tHP: \t\t{currentHP}/{maximumHP}");
            Utils.Add($"\tLevel: \t\t{level}");
            Utils.Add($"\tXP: \t\t{XP}/{XPToLevelUp}");
            Utils.Add($"\tGold: \t\t{gold}");
        }

        public void GainQuest(Quest quest)
        {
            if(quest.playerHasQuest || quest.complete)
            {
                return;
            }

            Utils.Add(quest.questGainedText);
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

            int damage = RandomNumberGenerator.NumberBetween(currentWeapon.minDamage, currentWeapon.maxDamage);
            enemToAttack.currentHP -= damage;
            Utils.Add($"You hit {enemToAttack.name.FullName} for {damage} damage!");

            if (enemToAttack.currentHP <= 0)
            {
                enemToAttack.Die(this);

                return;
            }

            enemToAttack.Attack(this);
        }
    }
}
