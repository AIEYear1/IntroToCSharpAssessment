namespace CRPGNamespace
{
    /// <summary>
    /// Quest variant for locations
    /// </summary>
    class QuestLocation : Location
    {
        /// <summary>
        /// Quest that this Item relates to
        /// </summary>
        public Quest relatingQuest;
        /// <summary>
        /// Objective that this item calls
        /// </summary>
        public int objectiveMarker;

        /// Parameters
        /// <param name="iD">ID reference for finding specific locations</param>
        /// <param name="name">Name of the location</param>
        /// <param name="description">Description of the location</param>
        /// <param name="relatingQuest">Quest that this item is relating to</param>
        /// <param name="objectiveMarker">Objective that this item calls (-1 to add quest)</param>
        public QuestLocation(int iD, string name, string description, Quest relatingQuest, int objectiveMarker) : base(iD, name, description)
        {
            this.name = name;
            this.description = description;
            this.relatingQuest = relatingQuest;
            this.objectiveMarker = objectiveMarker;
        }

        /// <summary>
        /// Calling quest for adding or continueing
        /// </summary>
        public void CallQuest()
        {
            if (objectiveMarker == -1)
            {
                Program.player.GainQuest(relatingQuest);
                return;
            }

            relatingQuest.ObjectiveMarker(objectiveMarker);
        }
    }
}
