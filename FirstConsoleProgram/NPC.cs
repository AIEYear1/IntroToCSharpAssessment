using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGThing
{
    class NPC
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
