using Assignment7_2_1.Domain;
using Assignment7_2_1.Contracts;
using Assignment7_2_1.Service.Client;
namespace Assignment7_2_1.Service.Application;
/// <summary>Requires the proposed participation student to be active.</summary>
public sealed class ActiveStudentRule : IParticipationAcceptanceRule
{
    public ParticipationAcceptanceResult Evaluate(ParticipationRecord record)
    {
        ArgumentNullException.ThrowIfNull(record);
        return record.Student.IsActive ? ParticipationAcceptanceResult.Accepted()
            : ParticipationAcceptanceResult.Rejected("The student is inactive.");
    }
}
