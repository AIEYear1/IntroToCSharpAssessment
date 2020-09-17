using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGNamespace
{
    public class Location
    {
        public int ID;
        public string name;
        public string description;
        public Item itemRequiredToEnter;
        public Monster monsterLivingHere;
        public NPC npcLivingHere;
        public Location locationToNorth;
        public Location locationToEast;
        public Location locationToWest;
        public Location locationToSouth;

        public bool properNoun;
        public bool knownNoun;

        public Location(int iD, string name, string description, bool knownNoun = false, bool properNoun = false)
        {
            ID = iD;
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
                Utils.Add($"There is {Utils.PrefixNoun(monsterLivingHere.Name.FullName, monsterLivingHere.ProperNoun, monsterLivingHere.KnownNoun, TextColor.RED)} here");
            }
            if(npcLivingHere != null)
            {
                Utils.Add($"There is{Utils.PrefixNoun(npcLivingHere.name.FullName, npcLivingHere.properNoun, npcLivingHere.knownNoun, TextColor.RED)} here");
            }
        }

        public void LookDirection(string dir)
        {
            switch (dir)
            {
                case "North":
                    if (locationToNorth == null)
                    {
                        Utils.Add("There is nothing to the North");
                        return;
                    }

                    locationToNorth.knownNoun = true;
                    Utils.Add(locationToNorth.description);
                    return;
                case "East":
                    if (locationToEast == null)
                    {
                        Utils.Add("There is nothing to the East");
                        return;
                    }

                    locationToEast.knownNoun = true;
                    Utils.Add(locationToEast.description);
                    return;
                case "South":
                    if (locationToSouth == null)
                    {
                        Utils.Add("There is nothing to the South");
                        return;
                    }

                    locationToSouth.knownNoun = true;
                    Utils.Add(locationToSouth.description);
                    return;
                case "West":
                    if (locationToWest == null)
                    {
                        Utils.Add("There is nothing to the West");
                        return;
                    }

                    locationToWest.knownNoun = true;
                    Utils.Add(locationToWest.description);
                    return;
            }
        }
    }
}
