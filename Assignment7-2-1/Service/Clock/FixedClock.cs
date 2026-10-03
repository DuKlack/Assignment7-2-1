namespace Assignment7_2_1.Service.Clock;

public class FixedClock:IClock
{
    
    public DateTime Now { get; }

    public FixedClock(DateTime fixedTime)
    {
        Now = fixedTime;
    }
    
}