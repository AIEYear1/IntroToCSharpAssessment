using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGThing
{
    public class Quest
    {
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

        public Quest(string name, string description, List<(string, string)> Objectives, int rewardGold, int rewardXP, string questGainedText, string completionText, Quest followUpQuest = null, bool mainQuest = false, bool complete = false)
        {
            this.name = name;
            this.description = description;
            this.questGainedText = questGainedText;
            this.rewardGold = rewardGold;
            this.rewardXP = rewardXP;
            this.mainQuest = mainQuest;
            this.complete = complete;
            this.completionText = completionText;
            this.followUpQuest = followUpQuest;
            foreach ((string name, string completionText) objective in Objectives)
            {
                objectives.Add(new Objective(objective.name, objectives.Count, objective.completionText));
            }
        }

        public void ObjectiveMarker(int objectivePoint)
        {
            if (objectivePoint < 0)
                objectivePoint = 0;

            if (objectivePoint != 0 && !objectives[objectivePoint - 1].Complete)
            {
                Utils.Add("Objective not ready to hit!");
                return;
            }
            if (objectives[objectivePoint].Complete)
            {
                Utils.Add("Objective already hit!");
                return;
            }

            objectives[objectivePoint].Complete = true;
            Utils.Add(Utils.ColorText(objectives[objectivePoint].CompletionText, Color.MAGENTA));

            foreach (Objective o in objectives)
            {
                if (!o.Complete)
                {
                    return;
                }
            }

            CompleteQuest();
        }

        public void CompleteQuest()
        {
            complete = true;
            Program.player.gold += rewardGold;
            Program.player.EarnXP(rewardXP);
            Utils.Add(Utils.ColorText(completionText, Color.MAGENTA));
            Utils.Add($"You gained {Utils.ColorText(rewardGold.ToString(), Color.YELLOW)} gold");
            Utils.Add($"You earned {Utils.ColorText(rewardXP.ToString(), Color.GREEN)} XP");

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
