using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGThing
{
    public class NPC
    {
        Name name;
        string talkLine;

        public NPC(Name name, string talkLine)
        {
            this.name = name;
            this.talkLine = talkLine;
        }
    }
}
