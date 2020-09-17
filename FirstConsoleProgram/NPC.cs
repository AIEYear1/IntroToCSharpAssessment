using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGNamespace
{
    public class NPC
    {
        public Name name;
        public string talkLine;
        readonly string description;
        public bool knownNoun;
        public bool properNoun;

        public NPC(Name name, string talkLine, string description, bool knownNoun, bool properNoun)
        {
            this.name = name;
            this.talkLine = talkLine;
            this.description = description;
            this.knownNoun = knownNoun;
            this.properNoun = properNoun;
        }

        public virtual void Talk()
        {
            Utils.Add(talkLine);
        }

        public void Look()
        {
            Utils.Add(name.FullName);
            Utils.Add(description);
        }
    }
}
