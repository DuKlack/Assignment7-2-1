namespace Assignment7_2_1.Service.Clock;

/// <summary>Provides a constant time supplied at construction.</summary>
public class FixedClock:IClock
{
    
    /// <summary>Gets the current time reported by this clock.</summary>
    public DateTime Now { get; }

    /// <summary>Initializes the clock with the fixed time it will always return.</summary>
    public FixedClock(DateTime fixedTime)
    {
        Now = fixedTime;
    }
    
}
