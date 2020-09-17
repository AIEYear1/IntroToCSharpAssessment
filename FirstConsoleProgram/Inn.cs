using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGNamespace
{
    class Inn : QueryNPC
    {
        readonly int price;

        public Inn(Name name, string talkLine, string description, string question, int price, bool knownNoun = false, bool properNoun = false) : base(name, talkLine, description, question, knownNoun, properNoun)
        {
            this.price = price;
        }

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
                    Program.player.SetHome();
                    break;
            }
        }
    }
}
