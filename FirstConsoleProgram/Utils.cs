using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

enum TextColor
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
    GOLD = 3,
    DARKRED = 88
}

struct Utils
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

    public static string PrefixNoun(string noun, bool nonCountNoun, bool nounKnown, TextColor color = TextColor.WHITE)
    {
        string vowels = "aeiou";

        if(color != TextColor.WHITE)
        {
            noun = ColorText(noun, color);
        }

        if (nounKnown)
        {
            return "the " + noun;
        }

        if (nonCountNoun)
        {
            return noun;
        }

        return (vowels.Contains(noun[0]) ? "an " : "a ") + noun;
    }

    public static string ColorText(string text, TextColor color)
    {
        return "\x1b[38;5;" + (int)color + "m" + text + "\x1b[38;5;255m";
    }


    public static Vector2 ClampMagnitude(Vector2 vector2, float magnitude)
    {
        if (vector2.Length() <= magnitude || vector2 == Vector2.Zero)
        {
            return vector2;
        }

        float x = magnitude / vector2.Length();
        return vector2 * x;
    }
    public static Vector2 LockMagnitude(Vector2 vector2, float magnitude)
    {
        if (vector2.Length() == magnitude || vector2 == Vector2.Zero)
        {
            return vector2;
        }

        float x = magnitude / vector2.Length();
        return vector2 * x;
    }

    public static float Lerp(float x, float y, float increment)
    {
        increment = MathF.Max(increment, 0);
        increment = MathF.Min(increment, 1);
        return x + ((y - x) * increment);
    }

    /// <summary>
    /// Returns an int based on a skewed chance
    /// </summary>
    /// <param name="values">The number of values that could be returned</param>
    /// <param name="probabilities">Array of chances for each value</param>
    /// <returns>Returns value between 0 and values-1</returns>
    public static int SkewedNum(int values, float[] probabilities)
    {
        Random rng = new Random();

        float totalP = 0, miscHold = 0;

        float[] oneToHundred = new float[probabilities.Length + 1];

        int[] valStorage = new int[values];
        for (int x = 0; x < values; x++)
            valStorage[x] = x;

        float[] augmentedP = probabilities;

        for (int counter = 0; counter < augmentedP.Length; counter++)
            if (augmentedP[counter] != -1)
                totalP += augmentedP[counter];

        for (int counter = 0; counter < augmentedP.Length; counter++)
            if (augmentedP[counter] != -1)
            {
                miscHold += augmentedP[counter];
                oneToHundred[counter + 1] = (miscHold / totalP) - 0.01f;
            }

        float hold = (float)rng.NextDouble();
        for (int counter = 1; counter < oneToHundred.Length; counter++)
            if (oneToHundred[counter - 1] <= hold && hold < oneToHundred[counter])
                return valStorage[counter - 1];

        return valStorage[0];
    }


    private static Random _generator = new Random(Guid.NewGuid().GetHashCode());

    public static int NumberBetween(int minVal, int maxVal)
    {
        return _generator.Next(minVal, maxVal + 1);
    }

    public static float AngleBetween(Vector2 vec1, Vector2 vec2)
    {
        return MathF.Acos(Vector2.Dot(vec1, vec2) / (vec1.Length() * vec2.Length())) * (180 / MathF.PI);
    }
}
