namespace SpravaVyrobkuaDilu.Core;

/// <summary>
/// Modified timer for internal needs
/// </summary>
public class CustomTimer
{
    /// <summary>
    /// Time of start
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    /// Current Time
    /// </summary>
    public DateTime Time2 { get; set; }

    /// <summary>
    /// Time difference
    /// </summary>
    public TimeSpan Difference { get; set; }

    /// <summary>
    /// IsRunning
    /// </summary>
    public bool Running { get; set; } = false;

    public void Start_timer()
    {
        if (!Running)
        {
            Time = DateTime.Now;
        }
        Running = true;
    }

    public bool Timer_enlapsed(double EnlapsedTime)
    {
        if (!Running)
        {
            throw new InvalidOperationException("Timer didnt start");
        }
        Time2 = DateTime.Now;
        Difference = Time2 - Time;
        if (Difference.TotalSeconds > EnlapsedTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Stop_timer()
    {
        Time = DateTime.MinValue;
        Time2 = DateTime.MinValue;
        Running = false;
    }
}