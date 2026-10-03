using Assignment7_2_1.Domain;
using Assignment7_2_1.Contracts;
using Assignment7_2_1.Service.Client;
namespace Assignment7_2_1.Service.Application;
/// <summary>Rejects repeated participation in the same category within ten minutes.</summary>
public sealed class DuplicateParticipationRule : IParticipationAcceptanceRule
{
    private readonly IParticipationRecordingData _data;
    public DuplicateParticipationRule(IParticipationRecordingData data) =>
        _data = data ?? throw new ArgumentNullException(nameof(data));
    public ParticipationAcceptanceResult Evaluate(ParticipationRecord record)
    {
        ArgumentNullException.ThrowIfNull(record);
        bool duplicate = _data.GetHistory(record.Student.Id).Any(previous =>
            previous.Category.Id == record.Category.Id &&
            Math.Abs((previous.OccurredAt - record.OccurredAt).TotalMinutes) < 10);
        return duplicate ? ParticipationAcceptanceResult.Rejected("This participation category is still in its cooldown period.")
            : ParticipationAcceptanceResult.Accepted();
    }
}
