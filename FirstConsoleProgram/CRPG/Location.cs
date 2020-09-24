namespace CRPGNamespace
{
    /// <summary>
    /// Base Location type
    /// </summary>
    public class Location
    {
        /// <summary>
        /// ID reference for finding specific locations
        /// </summary>
        public int ID;
        /// <summary>
        /// Name of the location
        /// </summary>
        public string name;
        /// <summary>
        /// Description of the location
        /// </summary>
        public string description;
        /// <summary>
        /// Monster that is held in the location
        /// </summary>
        public Monster monsterLivingHere;
        /// <summary>
        /// NPC that is held in the location
        /// </summary>
        public NPC npcLivingHere;
        /// <summary>
        /// Location to the north
        /// </summary>
        public Location locationToNorth;
        /// <summary>
        /// Location to the east
        /// </summary>
        public Location locationToEast;
        /// <summary>
        /// Location to the west
        /// </summary>
        public Location locationToWest;
        /// <summary>
        /// Location to the south
        /// </summary>
        public Location locationToSouth;
        /// <summary>
        /// Whether the noun is proper or normal
        /// </summary>
        public bool properNoun;
        /// <summary>
        /// Whether the noun is specific or abstract
        /// </summary>
        public bool knownNoun;

        /// Parameters
        /// <param name="iD">ID reference for finding specific locations</param>
        /// <param name="name">Name of the location</param>
        /// <param name="description">Description of the location</param>
        /// <param name="knownNoun">Whether the noun is Specific or abstract</param>
        /// <param name="properNoun">Whether the noun is proper or normal</param>
        public Location(int iD, string name, string description, bool knownNoun = false, bool properNoun = false)
        {
            ID = iD;
            this.name = name;
            this.description = description;
            this.knownNoun = knownNoun;
            this.properNoun = properNoun;
        }

        /// <summary>
        /// Gives a summary of everything in the location
        /// </summary>
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
                Utils.Add($"There is {Utils.PrefixNoun(npcLivingHere.name.FullName, npcLivingHere.properNoun, npcLivingHere.knownNoun, TextColor.SEAGREEN)} here");
            }
        }

        /// <summary>
        /// Looks in a specific direction
        /// </summary>
        /// <param name="dir">Direction to look</param>
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
