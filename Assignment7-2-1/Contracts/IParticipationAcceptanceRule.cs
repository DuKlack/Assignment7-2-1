using Assignment7_2_1.Domain;

namespace Assignment7_2_1.Contracts;

public interface IParticipationAcceptanceRule
{
    ParticipationAcceptanceResult Evaluate(ParticipationRecord record);
}
