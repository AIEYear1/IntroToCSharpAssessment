using System;
using System.Collections.Generic;
using System.Numerics;

/// <summary>
/// Enum which holds all of the possible colors text can be
/// </summary>
enum TextColor
{
    WHITE = 255,
    RED = 160,
    MAGENTA = 163,
    GREEN = 118,
    LIME = 154,
    YELLOW = 226,
    AQUA = 49,
    BLUE = 69,
    SALMON = 9,
    LIGHTBLUE = 14,
    GOLD = 3,
    DARKRED = 88,
    PINK = 199,
    SEAGREEN = 48
}

class Utils
{
    /// <summary>
    /// the string that is printed in Print()
    /// </summary>
    private static string outputToConsole = "";

    /// <summary>
    /// gets player input
    /// </summary>
    /// <returns>returns player input as a string</returns>
    public static string GetInput()
    {
        Console.Write(">");
        return Console.ReadLine().Trim().ToLower();
    }

    /// <summary>
    /// Asks the player a question
    /// </summary>
    /// <param name="question">question to ask the player</param>
    /// <returns>returns player input as a string</returns>
    public static string AskQuestion(string question)
    {
        Console.WriteLine(question);
        return GetInput();
    }

    /// <summary>
    /// Adds strToAdd to outputToConsole including an optional new line
    /// </summary>
    /// <param name="strToAdd">string to be added</param>
    /// <param name="addNewLine">whether to add a new line, assumed true</param>
    public static void Add(string strToAdd, bool addNewLine = true)
    {
        outputToConsole += strToAdd + (addNewLine ? "\n" : "");
    }

    /// <summary>
    /// prints outputToConsole to the console
    /// </summary>
    public static void Print()
    {
        Console.Write(outputToConsole);
        outputToConsole = "";
    }

    /// <summary>
    /// Prefixs the specified noun with the proper prefix
    /// </summary>
    /// <param name="noun">string to prefix</param>
    /// <param name="nonCountNoun">whether the noun is a NonCount Noun or a Count Noun</param>
    /// <param name="nounKnown">Whether the Noun is Known or abstract</param>
    /// <param name="color">Color to make the text, assumed White</param>
    /// <returns>returns the prefixed noun</returns>
    public static string PrefixNoun(string noun, bool nonCountNoun, bool nounKnown, TextColor color = TextColor.WHITE)
    {
        //A quick string of vowels for determining if the noun starts with a vowel
        string vowels = "aeiou";

        //if the color is n ot white change it
        if(color != TextColor.WHITE)
        {
            noun = ColorText(noun, color);
        }

        //if the noun is known return the + noun
        if (nounKnown)
        {
            return "the " + noun;
        }

        //if the noun is non count return noun
        if (nonCountNoun)
        {
            return noun;
        }

        //otherwise return a|an = noun
        return (vowels.Contains(noun[0]) ? "an " : "a ") + noun;
    }

    /// <summary>
    /// Changes text color
    /// </summary>
    /// <param name="text">string to change the color of</param>
    /// <param name="color">color to change the text to</param>
    /// <returns>Returns the text in the specified color</returns>
    public static string ColorText(string text, TextColor color)
    {
        return "\x1b[38;5;" + (int)color + "m" + text + "\x1b[38;5;255m";
    }

    /// <summary>
    /// ensures the vectors magnitude doesn't excede a certain magnitude
    /// </summary>
    /// <param name="vector2">Vector to clamp</param>
    /// <param name="magnitude">int to not exceed</param>
    /// <returns>Returns a vector with a magnitude no greater than what's specified</returns>
    public static Vector2 ClampMagnitude(Vector2 vector2, float magnitude)
    {
        if (vector2.Length() <= magnitude || vector2 == Vector2.Zero)
        {
            return vector2;
        }

        float x = magnitude / vector2.Length();
        return vector2 * x;
    }
    /// <summary>
    /// Locks a vectors magnitude
    /// </summary>
    /// <param name="vector2">Vector to lock</param>
    /// <param name="magnitude">int to lock to</param>
    /// <returns>returns a vector with the specified magnitude</returns>
    public static Vector2 LockMagnitude(Vector2 vector2, float magnitude)
    {
        if (vector2.Length() == magnitude || vector2 == Vector2.Zero)
        {
            return vector2;
        }

        float x = magnitude / vector2.Length();
        return vector2 * x;
    }

    /// <summary>
    /// lerps between two floats by a certain increment
    /// </summary>
    /// <param name="x">float to lerp from</param>
    /// <param name="y">float to lerp to</param>
    /// <param name="increment">float distance between both float</param>
    /// <returns>returns a float between x and y</returns>
    public static float Lerp(float x, float y, float increment)
    {
        increment = MathF.Max(increment, 0);
        increment = MathF.Min(increment, 1);
        return x + ((y - x) * increment);
    }


    private static readonly Random _generator = new Random(Guid.NewGuid().GetHashCode());
    /// <summary>
    /// Returns an int based on a skewed chance
    /// </summary>
    /// <param name="values">The number of values that could be returned</param>
    /// <param name="probabilities">Array of chances for each value</param>
    /// <returns>Returns value between 0 and values-1</returns>
    public static int SkewedNum(int values, float[] probabilities)
    {
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

        float hold = (float)_generator.NextDouble();
        for (int counter = 1; counter < oneToHundred.Length; counter++)
            if (oneToHundred[counter - 1] <= hold && hold < oneToHundred[counter])
                return valStorage[counter - 1];

        return valStorage[0];
    }

    /// <summary>
    /// get a random number between to numbers fully inclusive
    /// </summary>
    /// <param name="minVal">lowest returnable value</param>
    /// <param name="maxVal">highest returnable value</param>
    /// <returns>returns an int between the two numbers</returns>
    public static int NumberBetween(int minVal, int maxVal)
    {
        return _generator.Next(minVal, maxVal + 1);
    }

    /// <summary>
    /// finds the angle between to Vectors
    /// </summary>
    public static float AngleBetween(Vector2 vec1, Vector2 vec2)
    {
        float toReturn = MathF.Acos(Vector2.Dot(vec1, vec2) / (vec1.Length() * vec2.Length()));
        return RadToDeg(toReturn);
    }

    /// <summary>
    /// Converts from Degrees to Radians
    /// </summary>
    /// <param name="deg">Degree angle to convert</param>
    /// <returns>returns an agnle in radian</returns>
    public static float DegToRad(float deg)
    {
        deg = ((deg < 0) ? deg + 360 : deg);
        return deg * (MathF.PI / 180);
    }
    /// <summary>
    /// Converts from Radians to Degrees
    /// </summary>
    /// <param name="rad">Radian angle to convert</param>
    /// <returns>Returns an angle in degress</returns>
    public static float RadToDeg(float rad)
    {
        rad *= (180 / MathF.PI);
        return  + ((rad > 180) ? rad - 360 : rad);
    }

    /// <summary>
    /// Rotates A vector2 to an angle and sets the magnitude
    /// </summary>
    /// <param name="origin">Vector to rotate</param>
    /// <param name="angle">how far to rotate the angle in radians</param>
    /// <param name="magnitude">magnitude of rotated angle</param>
    /// <returns>returns an angle with specified angle and specified magnitude</returns>
    public static Vector2 RotationMatrix(Vector2 origin, float angle, float magnitude)
    {
        Vector2 toReturn = new Vector2((origin.X * MathF.Cos(angle)) + (origin.Y * MathF.Sin(angle)), (-origin.X * MathF.Sin(angle)) + (origin.Y * MathF.Cos(angle)));
        return toReturn * (magnitude / origin.Length());
    }

    /// <summary>
    /// Converts an Array into a string of the values separated by a splicable char
    /// </summary>
    /// <param name="array">Array to be converted</param>
    /// <param name="spacer">Spacer put in-between values from the Array</param>
    /// <returns>Returns a string of the values separated by a splicable char</returns>
    public static string ToString<TInput>(TInput[] array, string spacer = "")
    {
        string Return = "";
        for (int count = 0; count < array.Length; count++)
            Return += (array[count]) + (count + 1 < array.Length ? spacer : "");

        return Return;
    }

    /// <summary>
    /// Converts a List into a string of the values separated by a splicable char
    /// </summary>
    /// <param name="list">List to be converted</param>
    /// <param name="spacer">Spacer put in-between values from the List</param>
    /// <returns>Returns a string of the values separated by a splicable char</returns>
    public static string ToString<TInput>(List<TInput> list, string spacer = "")
    {
        string Return = "";

        for (int count = 0; count < list.Count; count++)
            Return += (list[count]) + (count + 1 < list.Count ? spacer : "");

        return Return;
    }
}
