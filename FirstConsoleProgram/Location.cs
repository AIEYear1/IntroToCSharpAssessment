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
        public Location locationtonorth;
        public Location locationtoeast;
        public Location locationtowest;
        public Location locationtosouth;
        public List<Location> LocationsAround = new List<Location>();

        public Location(string name, string description)
        {
            this.name = name;
            this.description = description;
        }
    }
}
