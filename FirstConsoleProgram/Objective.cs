using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGNamespace
{
    public class Objective
    {
        public string Name;
        public bool Complete = false;
        public int Marker;
        public string CompletionText;

        public Objective(string name, int marker, string completionText)
        {
            Name = name;
            Marker = marker;
            CompletionText = completionText;
        }
    }
}
