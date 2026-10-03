using Assignment7_2_1.Domain;
using Assignment7_2_1.Contracts;
using Assignment7_2_1.Service.Client;
namespace Assignment7_2_1.Service.Application;
/// <summary>Limits a student to three participation records per calendar day.</summary>
public sealed class DailyParticipationLimitRule : IParticipationAcceptanceRule
{
    private readonly IParticipationRecordingData _data;
    public DailyParticipationLimitRule(IParticipationRecordingData data) =>
        _data = data ?? throw new ArgumentNullException(nameof(data));
    public ParticipationAcceptanceResult Evaluate(ParticipationRecord record)
    {
        ArgumentNullException.ThrowIfNull(record);
        int count = _data.GetHistory(record.Student.Id).Count(previous => previous.OccurredAt.Date == record.OccurredAt.Date);
        return count >= 3 ? ParticipationAcceptanceResult.Rejected("The student has reached the daily participation limit.")
            : ParticipationAcceptanceResult.Accepted();
    }
}
