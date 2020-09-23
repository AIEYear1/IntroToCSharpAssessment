namespace CRPGNamespace
{
    /// <summary>
    /// Inn NPC
    /// </summary>
    class Inn : QueryNPC
    {
        /// <summary>
        /// Price to sleep at the inn
        /// </summary>
        readonly int price;

        /// Parameters
        /// <param name="name">Name of the NPC</param>
        /// <param name="talkLine">What the NPC says when you talk to them</param>
        /// <param name="description">Description of the NPC</param>
        /// <param name="question">Question they will ask you</param>
        /// <param name="price">Price to sleep in the inn</param>
        /// <param name="knownNoun">Whether the noun is specific or abstract</param>
        /// <param name="properNoun">Whether the noun is proper or not</param>
        public Inn(Name name, string talkLine, string description, string question, int price, bool knownNoun = false, bool properNoun = false) : base(name, talkLine, description, question, knownNoun, properNoun)
        {
            this.price = price;
        }

        /// <summary>
        /// Talk command allows player to talk to this NPC
        /// </summary>
        public override void Talk()
        {
            base.Talk();
            Utils.Print();
            switch (Utils.AskQuestion(question))
            {
                case "yes":
                case "y":
                    if(Program.player.gold < price)
                    {
                        Utils.Add("You don't have enough gold");
                        return;
                    }
                    Program.player.gold -= price;
                    Utils.Add("You sleep for the night");
                    Program.player.Home = Program.player.currentLocation;
                    break;
                case "no":
                case "n":
                    Utils.Add("Okay, talk to me again if you change your mind");
                    break;
                default:
                    Utils.Add("uh, alright?");
                    break;
            }
        }
    }
}
