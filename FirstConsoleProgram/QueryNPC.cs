using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGNamespace
{
    abstract class QueryNPC : NPC
    {
        public string question;

        public QueryNPC(Name name, string talkLine, string question, bool knownNoun = false, bool properNoun = false) : base(name, talkLine, knownNoun, properNoun)
        {
            this.question = question;
        }
    }
}
