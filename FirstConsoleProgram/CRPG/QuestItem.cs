namespace CRPGNamespace
{
    /// <summary>
    /// Quest variant for items
    /// </summary>
    class QuestItem : Item
    {
        /// <summary>
        /// Quest that this Item relates to
        /// </summary>
        public Quest relatingQuest;
        /// <summary>
        /// Objective that this item calls
        /// </summary>
        public int objectiveMarker;

        /// Paramters
        /// <param name="iD">Item ID for referencing</param>
        /// <param name="name">Item name</param>
        /// <param name="namePlural">Plural item name</param>
        /// <param name="description">Item description</param>
        /// <param name="relatingQuest">Quest that this item is relating to</param>
        /// <param name="objectiveMarker">Objective that this item calls (-1 to add quest)</param>
        /// <param name="value">Value of the item</param>
        public QuestItem(int iD, string name, string namePlural, string description, Quest relatingQuest, int objectiveMarker, int value) : base(iD, name, namePlural, description, value)
        {
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

            if (!Program.player.activeQuests.Contains(relatingQuest))
                return;

            relatingQuest.ObjectiveMarker(objectiveMarker);
        }
    }
}
