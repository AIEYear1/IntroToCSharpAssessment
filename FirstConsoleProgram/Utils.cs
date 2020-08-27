using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGThing
{
    enum Color
    {
        WHITE = 255,
        RED = 160,
        MAGENTA = 163,
        GREEN = 118,
        YELLOW = 226,
        AQUA = 49,
        BLUE = 69,
        SALMON = 9,
        LIGHTBLUE = 14,
        GOLD = 3
    }

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

        public static string PrefixNoun(string noun, bool properNounOrPlural, bool nounKnown, Color color = Color.WHITE)
        {
            string vowels = "aeiou";

            if(color != Color.WHITE)
            {
                noun = ColorText(noun, color);
            }

            if (nounKnown)
            {
                return "the " + noun;
            }

            if (properNounOrPlural)
            {
                return noun;
            }

            return (vowels.Contains(noun[0]) ? "an " : "a ") + noun;
        }

        public static string ColorText(string text, Color color)
        {
            return "\x1b[38;5;" + (int)color + "m" + text + "\x1b[38;5;255m";
        }
    }
}
