using static Raylib_cs.Raylib;

public struct Timer
{
    public readonly float delay;
    float timer;

    public Timer(float delay)
    {
        this.timer = 0;
        this.delay = delay;
    }

    public float PercentComplete
    {
        get
        {
            return timer / delay;
        }
    }
    public float CurrentTime
    {
        get
        {
            return timer;
        }
    }

    /// <summary>
    /// Reset the timer to the startPoint
    /// </summary>
    /// <param name="startPoint">The time in seconds you want the timer to get set to</param>
    public void Reset(float startPoint = 0)
    {
        timer = startPoint;
    }

    /// <summary>
    /// Increases timer by Time.deltaTime
    /// </summary>
    public void CountByTime()
    {
        timer += GetFrameTime();
    }

    /// <summary>
    /// Increases timer by value
    /// </summary>
    /// <param name="value">The float value you want to add to timer</param>
    public void CountByValue(float value)
    {
        timer += value;
    }

    /// <summary>
    /// Checks to see if the timer has reached or passed the delay
    /// </summary>
    /// <param name="resetOnTrue">Whether you want the timer to reset when IsComplete() is true</param>
    /// <returns>Returns true if timer is greater than or equal to delay</returns>
    public bool IsComplete(bool resetOnTrue = true)
    {
        if (timer >= delay)
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
    /// <param name="countByTime">Whether you want to increase the timer by Time.deltaTime or by value</param>
    /// <param name="value">The float value you want to add to timer if countByTime is false</param>
    /// <returns>Returns true if timer is greater than or equal to delay</returns>
    public bool Check(bool resetOnTrue = true)
    {
        if (IsComplete(resetOnTrue))
            return true;

        CountByTime();

        return false;
    }
    public bool Check(float value, bool resetOnTrue = true)
    {
        if (IsComplete(resetOnTrue))
            return true;

        CountByValue(value);

        return false;
    }
}
