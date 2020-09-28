using System;
using System.Numerics;

/// <summary>
/// Enum which holds all of the possible colors text can be
/// </summary>
enum TextColor
{
    WHITE = 255,
    YELLOW = 226,
    PINK = 199,
    MAGENTA = 163,
    RED = 160,
    LIME = 154,
    PURPLE = 90,
    DARKRED = 88,
    BLUE = 69,
    SEAGREEN = 48,
    GREEN = 46,
    LIGHTBLUE = 14,
    SALMON = 9,
    GOLD = 3
}

/// <summary>
/// A bunch of utility methods
/// </summary>
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
    public static void Add(string strToAdd)
    {
        outputToConsole += strToAdd + "\n";
    }
    /// <summary>
    /// Adds strToAdd to outputToConsole including an optional new line
    /// </summary>
    /// <param name="strToAdd">string to be added</param>
    /// <param name="color">wColor the text will be, if there is colored text inbetween, all text after the other colored text will be white</param>
    public static void Add(string strToAdd, TextColor color)
    {
        outputToConsole += Utils.ColorText(strToAdd, color) + "\n";
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
    /// <param name="properNoun">whether the noun is Proper or generic</param>
    /// <param name="nounKnown">Whether the Noun is Known or abstract</param>
    /// <param name="color">Color to make the text, assumed White</param>
    /// <returns>returns the prefixed noun</returns>
    public static string PrefixNoun(string noun, bool properNoun, bool nounKnown, TextColor color = TextColor.WHITE)
    {
        //A quick string of vowels for determining if the noun starts with a vowel
        string vowels = "aeiou";

        //If the color is not white change it
        if (color != TextColor.WHITE)
        {
            noun = ColorText(noun, color);
        }

        //If the noun is non count return noun
        if (properNoun)
        {
            return noun;
        }

        if (nounKnown)
        {
            return "the " + noun;
        }

        //Otherwise return a|an = noun
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

    //Random number generator for the random methods
    private static readonly Random _generator = new Random(Guid.NewGuid().GetHashCode());
    /// <summary>
    /// Returns an int based on a skewed chance, not used but I like the commenting in this so I'm keeping it to come back to at a later date
    /// </summary>
    /// <param name="values">The number of values that could be returned</param>
    /// <param name="probabilities">Array of chances for each value</param>
    /// <returns>Returns value between 0 and values-1</returns>
    public static int SkewedNum(int values, float[] probabilities)
    {
        ///Holds the total probability of anything accuring
        float totalProbability = 0;
        ///Temporarily holds the probability for each individual outcome
        float tmpProbability = 0;
        ///Holds a random number between o and 1, the end result is based off this
        float randomNum = Utils.NumberBetween(0, 100) / 100.0f;

        ///Holds probability of relating int ast a decimal between 0 and 1
        float[] augementedProbabilities = new float[probabilities.Length + 1];

        ///holds all of the possible outcomes
        int[] valStorage = new int[values];
        for (int x = 0; x < values; x++)
        {
            valStorage[x] = x;
        }

        //Sets the total probability
        for (int counter = 0; counter < probabilities.Length; counter++)
        {
            if (probabilities[counter] != -1)
            {
                totalProbability += probabilities[counter];
            }
        }

        //Sets the augemented probabilities
        for (int counter = 0; counter < probabilities.Length; counter++)
        {
            if (probabilities[counter] != -1)
            {
                tmpProbability += probabilities[counter];
                augementedProbabilities[counter + 1] = (tmpProbability / totalProbability) - 0.01f;
            }
        }

        //Finds the smallest probability that is less than the random number
        for (int counter = 1; counter < augementedProbabilities.Length; counter++)
        {
            if (augementedProbabilities[counter - 1] <= randomNum && randomNum < augementedProbabilities[counter])
            {
                return valStorage[counter - 1];
            }
        }

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
        return +((rad > 180) ? rad - 360 : rad);
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
}
