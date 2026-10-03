using Assignment7_2_1.Domain;
using Assignment7_2_1.Service.Application;

namespace Assignment7_2_1.Contracts;

public interface IParticipationAcceptanceRule
{
    ParticipationAcceptanceResult Evaluate(ParticipationRecord record);
}
