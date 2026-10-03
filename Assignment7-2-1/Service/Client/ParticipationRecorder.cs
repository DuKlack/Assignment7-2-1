using Assignment7_2_1.Domain;
using Assignment7_2_1.Contracts;
using Assignment7_2_1.Service.Client;
using Assignment7_2_1.Service.Clock;
namespace Assignment7_2_1.Service.Client;

/// <summary>Records participation only after all configured acceptance rules succeed.</summary>
public class ParticipationRecorder
{
    private readonly IParticipationRecordingData _data;
    private readonly IClock _clock;
    private readonly IReadOnlyList<IParticipationAcceptanceRule> _rules;
    public ParticipationRecorder(IParticipationRecordingData data, IClock clock, IReadOnlyList<IParticipationAcceptanceRule> rules)
    {
        _data = data ?? throw new ArgumentNullException(nameof(data));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
        ArgumentNullException.ThrowIfNull(rules);
        if (rules.Any(rule => rule is null))
            throw new ArgumentException("Rules cannot contain null entries.", nameof(rules));
        _rules = rules.ToArray();
    }
    public ParticipationAcceptanceResult RecordParticipation(Guid studentId, Guid categoryId, string? notes)
    {
        Student? student = _data.FindStudents().Find(item => item.Id == studentId);
        if (student is null) return ParticipationAcceptanceResult.Rejected("The student does not exist.");
        ParticipationCategory? category = _data.FindCategories().Find(item => item.Id == categoryId);
        if (category is null) return ParticipationAcceptanceResult.Rejected("The participation category does not exist.");
        if (notes is not null && notes.Length > 250)
            return ParticipationAcceptanceResult.Rejected("Participation notes cannot exceed 250 characters.");
        DateTime now = _clock.Now;
        ParticipationRecord proposal = new(Guid.NewGuid(), student, category, now, notes, now);
        foreach (IParticipationAcceptanceRule rule in _rules)
        {
            ParticipationAcceptanceResult result = rule.Evaluate(proposal);
            if (!result.IsAccepted) return result;
        }
        _data.StoreAccepted(proposal);
        return ParticipationAcceptanceResult.Accepted();
    }
}
