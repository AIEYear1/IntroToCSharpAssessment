namespace CRPGNamespace
{
    /// <summary>
    /// Abstract class for all NPCs that do more than just talk to the player
    /// </summary>
    abstract class QueryNPC : NPC
    {
        /// <summary>
        /// Question the NPC asks
        /// </summary>
        public string question;

        /// Parameters
        /// <param name="name">Name of the NPC</param>
        /// <param name="talkLine">Line the NPC speaks when talked to</param>
        /// <param name="description">Desciption of the NPC</param>
        /// <param name="question">Question the NPC asks</param>
        public QueryNPC(Name name, string talkLine, string description, string question, bool knownNoun = false, bool properNoun = false) : base(name, talkLine, description, knownNoun, properNoun)
        {
            this.question = question;
        }
    }
}
