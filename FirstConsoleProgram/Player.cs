using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CRPGThing
{
    public class Player : LivingCreature
    {
        public Name name;
        public int gold = 0;
        public int XP = 0;
        public int level = 1;
        public Weapon currentWeapon;
        public Armor currentArmor;
        public Location currentLocation;

        public Player(Name name, int gold, int xP, int level, Weapon currentWeapon, Armor currentArmor, Location currentLocation, int HP) : base(HP)
        {
            this.name = name;
            this.gold = gold;
            XP = xP;
            this.level = level;
            this.currentWeapon = currentWeapon;
            this.currentArmor = currentArmor;
            this.currentLocation = currentLocation;
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
                Utils.Add(enemToAttack.name.FullName + " has died");
                gold += enemToAttack.rewardGold;
                XP += enemToAttack.rewardXP;
                currentLocation.monsterLivingHere = null;
                return;
            }

            enemToAttack.Attack(this);
        }
    }
}
