using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGThing
{
    public class Location
    {
        public string name;
        public string description;
        public Item itemRequiredToEnter;
        public Monster monsterLivingHere;
        public Location locationToNorth;
        public Location locationToEast;
        public Location locationToWest;
        public Location locationToSouth;
        public List<Location> LocationsAround = new List<Location>();

        public bool properNoun;
        public bool knownNoun;

        public Location(string name, string description, bool knownNoun = false, bool properNoun = false)
        {
            this.name = name;
            this.description = description;
            this.knownNoun = knownNoun;
            this.properNoun = properNoun;
        }

        public void LookHere()
        {
            Utils.Add(description);

            if(locationToNorth != null)
            {
                Utils.Add("\tTo the North is " + Utils.PrefixNoun(locationToNorth.name, locationToNorth.properNoun, locationToNorth.knownNoun));
            }
            if (locationToEast != null)
            {
                Utils.Add("\tTo the East is " + Utils.PrefixNoun(locationToEast.name, locationToEast.properNoun, locationToEast.knownNoun));
            }
            if (locationToWest != null)
            {
                Utils.Add("\tTo the West is " + Utils.PrefixNoun(locationToWest.name, locationToWest.properNoun, locationToWest.knownNoun));
            }
            if (locationToSouth != null)
            {
                Utils.Add("\tTo the South is " + Utils.PrefixNoun(locationToSouth.name, locationToSouth.properNoun, locationToSouth.knownNoun));
            }
            if(monsterLivingHere != null)
            {
                Utils.Add($"There is a {monsterLivingHere.name.FullName} living here");
            }
        }

        public void LookNorth()
        {
            if(locationToNorth == null)
            {
                Utils.Add("There is nothing to the North");
                return;
            }

            Utils.Add(locationToNorth.description);
        }
        public void LookEast()
        {
            if (locationToEast == null)
            {
                Utils.Add("There is nothing to the East");
                return;
            }

            Utils.Add(locationToEast.description);
        }
        public void LookSouth()
        {
            if (locationToSouth == null)
            {
                Utils.Add("There is nothing to the South");
                return;
            }

            Utils.Add(locationToSouth.description);
        }
        public void LookWest()
        {
            if (locationToWest == null)
            {
                Utils.Add("There is nothing to the West");
                return;
            }

            Utils.Add(locationToWest.description);
        }
    }
}
