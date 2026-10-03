namespace Assignment7_2_1.Domain;

/// <summary>Reports whether a proposed participation record passed all rules.</summary>
public sealed class ParticipationAcceptanceResult
{
    private ParticipationAcceptanceResult(bool isAccepted, string? rejectionReason)
    {
        IsAccepted = isAccepted;
        RejectionReason = rejectionReason;
    }

    /// <summary>Gets whether participation was accepted.</summary>
    public bool IsAccepted { get; }
    /// <summary>Gets the rejection explanation, or null for an accepted result.</summary>
    public string? RejectionReason { get; }

    /// <summary>Creates an accepted result with no rejection reason.</summary>
    public static ParticipationAcceptanceResult Accepted()
    {
        return new ParticipationAcceptanceResult(true, null);
    }

    /// <summary>Creates a rejected result with a required nonblank explanation.</summary>
    /// <exception cref="ArgumentException">The reason is blank.</exception>
    public static ParticipationAcceptanceResult Rejected(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException("A rejection reason is required.", nameof(reason));
        }

        return new ParticipationAcceptanceResult(false, reason);
    }
}
