using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGThing
{
    public class Objective
    {
        public string Name;
        public bool Complete;
        public int Marker;

        public Objective(string name, int marker, bool complete = false)
        {
            Name = name;
            Marker = marker;
            Complete = complete;
        }
    }
}
