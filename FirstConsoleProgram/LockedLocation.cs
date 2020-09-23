namespace CRPGNamespace
{
    /// <summary>
    /// Holds all possible Locked Locations
    /// </summary>
    enum LockedLocationIndex
    {
        FORESTEDGE,
        NORTH
    }

    /// <summary>
    /// Locked Location location type
    /// </summary>
    class LockedLocation : Location
    {
        //Whether the player can move to the location
        bool canEnter = false;
        //What location type it is
        readonly LockedLocationIndex index;
        //Text that plays when the player can't move to the location
        readonly string lockedText;

        /// Parameters
        /// <param name="iD">ID reference for finding specific locations</param>
        /// <param name="name">Name of the location</param>
        /// <param name="description">Description of the location</param>
        /// <param name="locationIndex">What location type it is</param>
        /// <param name="lockedText">Text that plays when the player can't move to the location</param>
        public LockedLocation(int iD, string name, string description, LockedLocationIndex locationIndex, string lockedText) : base(iD, name, description)
        {
            this.index = locationIndex;
            this.lockedText = lockedText;
        }

        /// <summary>
        /// Checks to see if the player can enter the location
        /// </summary>
        /// <returns>returns true if the player can enter the Location</returns>
        public bool Enter()
        {
            switch (index)
            {
                case LockedLocationIndex.FORESTEDGE:
                    if(Program.player.Level < 2)
                        break;

                    canEnter = true;
                    Utils.Add("You unlock " + Utils.PrefixNoun(name, properNoun, knownNoun));
                    break;
                case LockedLocationIndex.NORTH:
                    if (Program.player.completedQuests.Contains(World.QuestByID(World.QUEST_ID_TUTORIALQUEST)))
                    {
                        Utils.Add("This is the current end of the demo");
                        return false;
                    }
                    break;
            }

            if (!canEnter)
            {
                Utils.Add(lockedText);
                return false;
            }

            return true;
        }
    }
}
