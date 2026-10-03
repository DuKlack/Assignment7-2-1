
namespace Assignment7_2_1.Service.Clock;

/// <summary>Provides the actual system time.</summary>
public class SystemClock:IClock
{
    /// <summary>Gets the current time reported by this clock.</summary>
    public DateTime Now => DateTime.Now;
}
