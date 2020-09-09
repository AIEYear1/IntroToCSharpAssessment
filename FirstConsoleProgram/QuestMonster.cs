using System;
using System.Collections.Generic;
using System.Text;
using RaylibWindowNamespace;

namespace CRPGNamespace
{
    class QuestMonster : Monster
    {
        public Quest relatingQuest;
        public int objectiveMarker;

        public QuestMonster(Name name, string description, int HP, EnemyAttack enemyAttack, int rewardXP, int rewardGold, Quest relatingQuest, int objectiveMarker, bool knownNoun = false, bool properNoun = false) : base(name, description, HP, enemyAttack, rewardXP, rewardGold, knownNoun, properNoun)
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
