using static Raylib_cs.Raylib;

/// <summary>
/// Used as an actual timer or can be used as a value counter
/// </summary>
public struct Timer
{
    /// <summary>
    /// How long the timer is
    /// </summary>
    public readonly float delay;
    /// <summary>
    /// The current time value of the Timer
    /// </summary>
    public float Time { get;  private set; }
    /// <summary>
    /// The percent of time left in the timer
    /// </summary>
    public float PercentComplete
    {
        get => Time / delay;
    }
    /// <summary>
    /// how much time is left in the timer
    /// </summary>
    public float TimeRemaining
    {
        get => delay - Time;
    }

    /// <summary>
    /// Creates a timer where Time is set to 0 and the length is delay
    /// </summary>
    /// <param name="delay">how long the timer will be</param>
    public Timer(float delay)
    {
        this.Time = 0;
        this.delay = delay;
    }


    /// <summary>
    /// Reset the timer to the startPoint
    /// </summary>
    /// <param name="startPoint">The time in seconds you want the timer to get set to</param>
    public void Reset(float startPoint = 0)
    {
        Time = startPoint;
    }

    /// <summary>
    /// Increases timer by Time.deltaTime
    /// </summary>
    public void CountByTime()
    {
        Time += GetFrameTime();
    }

    /// <summary>
    /// Increases timer by value
    /// </summary>
    /// <param name="value">The float value you want to add to timer</param>
    public void CountByValue(float value)
    {
        Time += value;
    }

    /// <summary>
    /// Checks to see if the timer has reached or passed the delay
    /// </summary>
    /// <param name="resetOnTrue">Whether you want the timer to reset when IsComplete() is true</param>
    /// <returns>Returns true if timer is greater than or equal to delay</returns>
    public bool IsComplete(bool resetOnTrue = true)
    {
        if (Time >= delay)
        {
            if (resetOnTrue)
                Reset();

            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks whether the timer has reached or passed the delay and if not count up
    /// </summary>
    /// <param name="resetOnTrue">Whether you want the timer to reset when IsComplete() is true</param>
    /// <returns>Returns true if timer is greater than or equal to delay</returns>
    public bool Check(bool resetOnTrue = true)
    {
        if (IsComplete(resetOnTrue))
            return true;

        CountByTime();

        return false;
    }

    /// <summary>
    /// Checks whether the timer has reached or passed the delay and if not count up by value
    /// </summary>
    /// <param name="value">value to count up by</param>
    /// <param name="resetOnTrue">Whether you want the timer to reset when IsComplete() is true</param>
    /// <returns>Returns true if timer is greater than or equal to delay</returns>
    public bool Check(float value, bool resetOnTrue = true)
    {
        if (IsComplete(resetOnTrue))
            return true;

        CountByValue(value);

        return false;
    }
}
