namespace CRPGNamespace
{
    class QuestNPC : NPC
    {
        /// <summary>
        /// Quest that this Item relates to
        /// </summary>
        public Quest relatingQuest;
        /// <summary>
        /// Objective that this item calls
        /// </summary>
        public int objectiveMarker;
        /// <summary>
        /// Line NPC speaks after hitting the objective the NPC calls
        /// </summary>
        readonly string postQuestTalkLine;

        /// Parameters
        /// <param name="name">Name of the NPC</param>
        /// <param name="talkLine">Line the NPC speaks when talked to</param>
        /// <param name="postQuestTalkLine">Line NPC speaks after hitting the objective the NPC calls</param>
        /// <param name="description">Desciption of the NPC</param>
        /// <param name="relatingQuest">Quest that this item is relating to</param>
        /// <param name="objectiveMarker">Objective that this item calls (-1 to add quest)</param>
        public QuestNPC(Name name, string talkLine, string postQuestTalkLine, string description, Quest relatingQuest, int objectiveMarker, bool knownNoun = false, bool properNoun = false) : base(name, talkLine, description, knownNoun, properNoun)
        {
            this.relatingQuest = relatingQuest;
            this.objectiveMarker = objectiveMarker;
            this.postQuestTalkLine = postQuestTalkLine;
        }

        /// <summary>
        /// What happens when you talk to the NPC
        /// </summary>
        public override void Talk()
        {
            base.Talk();
            CallQuest();
            talkLine = postQuestTalkLine;
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
