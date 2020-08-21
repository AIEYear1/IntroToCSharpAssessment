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

        public Location(string name, string description)
        {
            this.name = name;
            this.description = description;
        }

        public void LookHere()
        {
            Utils.Add(description);

            if(locationToNorth != null)
            {
                Utils.Add("\tTo the North is " + locationToNorth.name);
            }
            if (locationToEast != null)
            {
                Utils.Add("\tTo the East is " + locationToEast.name);
            }
            if (locationToWest != null)
            {
                Utils.Add("\tTo the West is " + locationToWest.name);
            }
            if (locationToSouth != null)
            {
                Utils.Add("\tTo the South is " + locationToSouth.name);
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
