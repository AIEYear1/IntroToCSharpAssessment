using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGNamespace
{
    public class Quest
    {
        public int ID;
        public string name;
        public string description;
        public List<Objective> objectives = new List<Objective>();
        public string questGainedText;
        public int rewardGold;
        public int rewardXP;
        public List<LootItem> lootTable = new List<LootItem>();
        public bool complete;
        public bool mainQuest;
        public bool playerHasQuest = false;
        public string completionText;
        public Quest followUpQuest;

        public Quest(int iD, string name, string description, List<(string name, string completionText)> Objectives, int rewardGold, int rewardXP, string questGainedText, string completionText, Quest followUpQuest = null, bool mainQuest = false, bool complete = false)
        {
            ID = iD;
            this.name = name;
            this.description = description;
            this.questGainedText = questGainedText;
            this.rewardGold = rewardGold;
            this.rewardXP = rewardXP;
            this.mainQuest = mainQuest;
            this.complete = complete;
            this.completionText = completionText;
            this.followUpQuest = followUpQuest;
            for(int x = 0; x < Objectives.Count; x++)
            {
                objectives.Add(new Objective(Objectives[x].name, objectives.Count, Objectives[x].completionText));
            }
        }

        public void ObjectiveMarker(int objectivePoint)
        {
            if (objectivePoint == -1)
                return;
            if (objectivePoint < 0)
                objectivePoint = 0;
            if (objectives[objectivePoint].Complete)
            {
                return;
            }

            for (int x = 0; x <= objectivePoint; x++)
            {
                objectives[x].Complete = true;
            }
            Utils.Add(Utils.ColorText(objectives[objectivePoint].CompletionText, TextColor.MAGENTA));

            for(int x = 0; x<objectives.Count; x++)
            {
                if(!objectives[x].Complete)
                    return;
            }

            CompleteQuest();
        }

        public void CompleteQuest()
        {
            complete = true;
            Program.player.gold += rewardGold;
            Program.player.EarnXP(rewardXP);
            Utils.Add(Utils.ColorText(completionText, TextColor.MAGENTA));
            Utils.Add($"You gained {Utils.ColorText(rewardGold.ToString(), TextColor.YELLOW)} gold");
            Utils.Add($"You earned {Utils.ColorText(rewardXP.ToString(), TextColor.GREEN)} XP");

            if(followUpQuest != null)
            {
                Program.player.GainQuest(followUpQuest);
            }
        }

        public void LookQuest()
        {
            Utils.Add(name);
            Utils.Add(description);
            foreach (Objective o in objectives)
            {
                if (!o.Complete)
                {
                    Utils.Add("\t" + o.Name);
                    break;
                }

                Utils.Add("\t" + o.Name);
            }
        }
    }
}
