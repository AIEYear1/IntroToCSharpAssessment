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
    }
}
