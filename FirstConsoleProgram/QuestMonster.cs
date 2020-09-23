using RaylibWindowNamespace;

namespace CRPGNamespace
{
    /// <summary>
    /// Quest variant for Monsters
    /// </summary>
    class QuestMonster : Monster
    {
        /// <summary>
        /// Quest that this Item relates to
        /// </summary>
        public Quest relatingQuest;
        /// <summary>
        /// Objective that this item calls
        /// </summary>
        public int objectiveMarker;

        /// Parameters
        /// <param name="name">Name of the monster</param>
        /// <param name="description">Description of the monster</param>
        /// <param name="HP">Health of the monster</param>
        /// <param name="enemyAttack">Attack the enemy uses</param>
        /// <param name="rewardXP">XP earned from defeating the monster</param>
        /// <param name="rewardGold">Gold earned from defeating the monster</param>
        /// <param name="relatingQuest">Quest that this item is relating to</param>
        /// <param name="objectiveMarker">Objective that this item calls (-1 to add quest)</param>
        public QuestMonster(Name name, string description, int HP, EnemyAttack enemyAttack, int rewardXP, int rewardGold, Quest relatingQuest, int objectiveMarker, bool knownNoun = false, bool properNoun = false) : base(name, description, HP, enemyAttack, rewardXP, rewardGold, knownNoun, properNoun)
        {
            this.relatingQuest = relatingQuest;
            this.objectiveMarker = objectiveMarker;
        }

        /// <summary>
        /// Calling quest for adding or continueing
        /// </summary>
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
