using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGThing
{
    class QuestMonster : Monster
    {
        public Quest relatingQuest;
        public int objectiveMarker;

        public QuestMonster(Name name, string description, int HP, int maximumDamage, int minimumDamage, int rewardXP, int rewardGold, Quest relatingQuest, int objectiveMarker) : base(name, description, HP, maximumDamage, minimumDamage, rewardXP, rewardGold)
        {
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
