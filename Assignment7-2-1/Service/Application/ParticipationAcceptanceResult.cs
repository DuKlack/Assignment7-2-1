namespace Assignment7_2_1.Service.Application;

/// <summary>Reports whether a proposed participation record passed all rules.</summary>
public sealed class ParticipationAcceptanceResult
{
    private ParticipationAcceptanceResult(bool isAccepted, string? rejectionReason)
    {
        IsAccepted = isAccepted;
        RejectionReason = rejectionReason;
    }

    public bool IsAccepted { get; }
    public string? RejectionReason { get; }

    public static ParticipationAcceptanceResult Accepted()
    {
        return new ParticipationAcceptanceResult(true, null);
    }

    public static ParticipationAcceptanceResult Rejected(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException("A rejection reason is required.", nameof(reason));
        }

        return new ParticipationAcceptanceResult(false, reason);
    }
}
