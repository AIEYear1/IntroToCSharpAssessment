using System.Collections.Generic;

namespace CRPGNamespace
{
    /// <summary>
    /// Quests the player will follow throughout the game
    /// </summary>
    public class Quest
    {
        /// <summary>
        /// ID of the quest for referencing
        /// </summary>
        public int ID;
        /// <summary>
        /// Name of the Quest
        /// </summary>
        public string name;
        /// <summary>
        /// Description of the Quest
        /// </summary>
        public string description;
        /// <summary>
        /// Text shown when player gains the quest
        /// </summary>
        public string questGainedText;
        /// <summary>
        /// Whether the quest is a main quest or a side quest
        /// </summary>
        public bool mainQuest;
        /// <summary>
        /// Gold the player will gain for completing the quest
        /// </summary>
        public int rewardGold;
        /// <summary>
        /// XP the player will gain from completing the quest
        /// </summary>
        public int rewardXP;
        /// <summary>
        /// Text shown when the player completes the quest
        /// </summary>
        public string completionText;
        /// <summary>
        /// The quest the player would get once they finish this quest
        /// </summary>
        public Quest followUpQuest;
        /// <summary>
        /// List of all the Quest objectives
        /// </summary>
        public Objective[] objectives;
        /// <summary>
        /// List of all possible items the player could earn from completing this quest
        /// </summary>
        public List<LootItem> lootTable = new List<LootItem>();
        /// <summary>
        /// Whether the quest is complete
        /// </summary>
        public bool complete = false;
        /// <summary>
        /// Whether the player has this quest
        /// </summary>
        public bool playerHasQuest = false;

        /// Parameters
        /// <param name="iD">ID of the quest for referencing</param>
        /// <param name="name">Name of the quest</param>
        /// <param name="description">Description of the quest</param>
        /// <param name="Objectives">List of all objectives in said quest</param>
        /// <param name="rewardGold">Gold the player earns when the quest is completed</param>
        /// <param name="rewardXP">XP the player earns when the quest is complete</param>
        /// <param name="questGainedText">Text shown when the player gains the quest</param>
        /// <param name="completionText">Text shown when the player completes the quest</param>
        /// <param name="mainQuest">Whether the quest is a main quest or side quest (assumed false)</param>
        public Quest(int iD, string name, string description, int rewardGold, int rewardXP, string questGainedText, string completionText, bool mainQuest = false)
        {
            ID = iD;
            this.name = name;
            this.description = description;
            this.questGainedText = questGainedText;
            this.rewardGold = rewardGold;
            this.rewardXP = rewardXP;
            this.mainQuest = mainQuest;
            this.completionText = completionText;
        }

        /// <summary>
        /// Attemots to complete a specified objective
        /// </summary>
        /// <param name="objectivePoint">Objective to attempt to complete</param>
        public void ObjectiveMarker(int objectivePoint)
        {
            //used for Quest adding
            if (objectivePoint == -1)
                return;

            //Checks to ensure Objective isn't already complee
            if (objectives[objectivePoint].Complete)
            {
                return;
            }

            //Completes all objectives before this marker
            for (int x = 0; x <= objectivePoint; x++)
            {
                if (objectives[x].Complete)
                    continue;

                Objective tmpObjective = objectives[x];
                tmpObjective.Complete = true;
                objectives[x] = tmpObjective;
            }
            Utils.Add(Utils.ColorText(objectives[objectivePoint].CompletionText, TextColor.MAGENTA));

            //Checks to see if there are any objectives the player hasn't completed
            for(int x = 0; x<objectives.Length; x++)
            {
                if(!objectives[x].Complete)
                    return;
            }

            CompleteQuest();
        }

        /// <summary>
        /// Competes the quest and gives the player their reward (Loot table not currently implemented)
        /// </summary>
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
            Program.player.completedQuests.Add(this);
            Program.player.activeQuests.Remove(this);
        }

        /// <summary>
        /// Popup for when the player looks at a specific quest
        /// </summary>
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
