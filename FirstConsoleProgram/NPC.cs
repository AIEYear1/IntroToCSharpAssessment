using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGNamespace
{
    public class NPC
    {
        public Name name;
        readonly string talkLine;
        public bool knownNoun;
        public bool properNoun;

        public NPC(Name name, string talkLine, bool knownNoun, bool properNoun)
        {
            this.name = name;
            this.talkLine = talkLine;
            this.knownNoun = knownNoun;
            this.properNoun = properNoun;
        }

        public virtual void Talk()
        {
            Utils.Add(talkLine);
        }
    }
}
