using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGThing
{
    class Utils
    {
        private static string outputToConsole = "";

        public static string GetInput()
        {
            Console.Write(">");
            return Console.ReadLine().Trim().ToLower();
        }

        public static string AskQuestion(string question)
        {
            Console.WriteLine(question);
            return GetInput();
        }

        public static void Add(string strToAdd, bool addNewLine = true)
        {
            outputToConsole += strToAdd + (addNewLine ? "\n" : "");
        }

        public static void Print()
        {
            Console.Write(outputToConsole);
            outputToConsole = "";
        }
    }
}
