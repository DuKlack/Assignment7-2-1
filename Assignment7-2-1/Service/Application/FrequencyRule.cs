using Assignment7_2_1.Contracts;
using Assignment7_2_1.Domain;

namespace Assignment7_2_1.Service.Application;

public class FrequencyRule : ParticipationAcceptanceRule
{
    private readonly IParticipationRecordRepository _recordRepository;
    private readonly TimeSpan _cooldown = TimeSpan.FromMinutes(10);
    private readonly int _dailyLimit = 3;
    public FrequencyRule(Guid id, string name, string instructor, IParticipationRecordRepository recordRepository) : base(name, instructor, id) => _recordRepository = recordRepository ?? throw new ArgumentNullException(nameof(recordRepository));
    public override string GetRuleType() => nameof(FrequencyRule);
    public override ParticipationAcceptanceResult Evaluate(ParticipationRecord record)
    {
        ArgumentNullException.ThrowIfNull(record);
        List<ParticipationRecord> records = _recordRepository.GetAll();
        DateTime dayStart = record.OccurredAt.Date;
        DateTime dayEnd = dayStart.AddDays(1);
        int dailyCount = records.Count(item => item.Student.Id == record.Student.Id && item.OccurredAt >= dayStart && item.OccurredAt < dayEnd);
        if (dailyCount >= _dailyLimit) return ParticipationAcceptanceResult.Rejected("The student has reached the daily participation limit.");
        bool inCooldown = records.Any(item => item.Student.Id == record.Student.Id && item.Category.Id == record.Category.Id && Math.Abs((item.OccurredAt - record.OccurredAt).TotalMinutes) < _cooldown.TotalMinutes);
        return inCooldown ? ParticipationAcceptanceResult.Rejected("This participation category is still in its cooldown period.") : ParticipationAcceptanceResult.Accepted();
    }
}
