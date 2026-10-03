using Assignment7_2_1.Contracts;
using Assignment7_2_1.Domain;

namespace Assignment7_2_1.Service.Application;

public class ParticipationRecordingService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IParticipationCategoryRepository _categoryRepository;
    private readonly IParticipationRecordRepository _recordRepository;
    private readonly List<IParticipationAcceptanceRule> _rules;
    public ParticipationRecordingService(IStudentRepository studentRepository, IParticipationCategoryRepository categoryRepository, IParticipationRecordRepository recordRepository, IEnumerable<IParticipationAcceptanceRule> rules)
    {
        _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        _recordRepository = recordRepository ?? throw new ArgumentNullException(nameof(recordRepository));
        ArgumentNullException.ThrowIfNull(rules);
        _rules = rules.ToList();
    }
    public ParticipationAcceptanceResult RecordParticipation(Guid studentId, Guid categoryId, string? notes, DateTime occurredAt)
    {
        Student student;
        ParticipationCategory category;

        try
        {
            student = _studentRepository.GetById(studentId);
        }
        catch (KeyNotFoundException)
        {
            return ParticipationAcceptanceResult.Rejected("The student does not exist.");
        }

        try
        {
            category = _categoryRepository.GetById(categoryId);
        }
        catch (KeyNotFoundException)
        {
            return ParticipationAcceptanceResult.Rejected("The participation category does not exist.");
        }

        ParticipationRecord proposedRecord = new(Guid.NewGuid(), student, category, occurredAt, notes);
        foreach (IParticipationAcceptanceRule rule in _rules)
        {
            ParticipationAcceptanceResult result = rule.Evaluate(proposedRecord);
            if (!result.IsAccepted) return result;
        }
        _recordRepository.Add(proposedRecord);
        return ParticipationAcceptanceResult.Accepted();
    }
}
