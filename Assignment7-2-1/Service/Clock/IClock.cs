namespace Assignment7_2_1.Service.Clock;

/// <summary>Provides the current time independently of its source.</summary>
public interface IClock
{
    /// <summary>Gets the current time reported by this clock.</summary>
    DateTime Now { get; }
}
