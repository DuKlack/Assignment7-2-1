using Assignment7_2_1.Domain;

namespace Assignment7_2_1.Contracts;

/// <summary>Evaluates a proposed record and returns acceptance or a clear rejection reason.</summary>
public interface IParticipationAcceptanceRule
{
    /// <summary>Evaluates the proposed record without storing it and explains any rejection.</summary>
    ParticipationAcceptanceResult Evaluate(ParticipationRecord record);
}
