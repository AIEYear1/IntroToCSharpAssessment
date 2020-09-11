using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGNamespace
{
    class QuestLocation : Location
    {
        public Quest relatingQuest;
        public int objectiveMarker;

        public QuestLocation(int iD, string name, string description, Quest relatingQuest, int objectiveMarker) :base(iD, name, description)
        {
            this.name = name;
            this.description = description;
            this.relatingQuest = relatingQuest;
            this.objectiveMarker = objectiveMarker;
        }

        public void CallQuest()
        {
            if(objectiveMarker == -1)
            {
                Program.player.GainQuest(relatingQuest);
                return;
            }

            relatingQuest.ObjectiveMarker(objectiveMarker);
        }
    }
}
