using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGNamespace
{
    class QuestNPC : NPC
    {
        readonly Quest relatingQuest;
        readonly int objectiveMarker;
        readonly string postQuestTalkLine;

        public QuestNPC(Name name, string talkLine, string postQuestTalkLine, string description, Quest relatingQuest, int objectiveMarker, bool knownNoun = false, bool properNoun = false) : base(name, talkLine, description, knownNoun, properNoun)
        {
            this.relatingQuest = relatingQuest;
            this.objectiveMarker = objectiveMarker;
            this.postQuestTalkLine = postQuestTalkLine;
        }

        public override void Talk()
        {
            base.Talk();
            CallQuest();
            talkLine = postQuestTalkLine;
        }

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
