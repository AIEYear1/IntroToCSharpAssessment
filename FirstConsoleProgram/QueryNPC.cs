using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGNamespace
{
    abstract class QueryNPC : NPC
    {
        public string question;

        public QueryNPC(Name name, string talkLine, string description, string question, bool knownNoun = false, bool properNoun = false) : base(name, talkLine, description, knownNoun, properNoun)
        {
            this.question = question;
        }
    }
}
