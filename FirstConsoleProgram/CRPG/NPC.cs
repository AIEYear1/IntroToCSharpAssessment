namespace CRPGNamespace
{
    /// <summary>
    /// NPCs that appear around the world
    /// </summary>
    public class NPC
    {
        /// <summary>
        /// Name of the NPC
        /// </summary>
        public Name name;
        /// <summary>
        /// Line NPC speaks when talked to
        /// </summary>
        public string talkLine;
        /// <summary>
        /// Description of the NPC
        /// </summary>
        readonly string description;
        /// <summary>
        /// Whether the NPC is known or abstract
        /// </summary>
        public bool knownNoun;
        /// <summary>
        /// Whether the NPC name is Proper or generic
        /// </summary>
        public bool properNoun;

        /// Parameters
        /// <param name="name">Name of the NPC</param>
        /// <param name="talkLine">Line the NPC speaks when talked to</param>
        /// <param name="description">Desciption of the NPC</param>
        /// <param name="knownNoun">Whether the NPC is known or abstract</param>
        /// <param name="properNoun">Whether the NPC Name is Proper or generic</param>
        public NPC(Name name, string talkLine, string description, bool knownNoun, bool properNoun)
        {
            this.name = name;
            this.talkLine = talkLine;
            this.description = description;
            this.knownNoun = knownNoun;
            this.properNoun = properNoun;
        }

        /// <summary>
        /// What happens when you talk to the NPC
        /// </summary>
        public virtual void Talk()
        {
            Utils.Add(talkLine);
        }

        /// <summary>
        /// Text popup when looking at an NPC
        /// </summary>
        public virtual void Look()
        {
            Utils.Add(name.FullName);
            Utils.Add(description);
        }
    }
}
