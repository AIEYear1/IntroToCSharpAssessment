using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGThing
{
    class QuestItem : Item
    {
        public Quest relatingQuest;
        public int objectiveMarker;

        public QuestItem(string name, string namePlural, string description, Quest relatingQuest, int objectiveMarker, int weight) :base(name, namePlural, description, weight)
        {
            this.relatingQuest = relatingQuest;
            this.objectiveMarker = objectiveMarker;
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
