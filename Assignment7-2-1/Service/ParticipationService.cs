using Assignment7_2_1.Domain;
using Assignment7_2_1.Service.Client;
using Assignment7_2_1.Service.Clock;    
using Assignment7_2_1.Service.Application;
using Assignment7_2_1.Contracts;
namespace Assignment7_2_1.Service;

/// <summary>Records participation using injected lookup, writing, rule, and clock dependencies.</summary>
public class ParticipationService
{
    private readonly IStudentLookup _students;
    private readonly ICategoryLookup _categories;
    private readonly IParticipationRecordWriter _writer;
    private readonly IReadOnlyList<IParticipationAcceptanceRule> _rules;
    private readonly IParticipationHistory _history;
    private readonly IClock _clock;
    /// <summary>Initializes ParticipationService with its required collaborators.</summary>
    public ParticipationService(
        IStudentLookup students,
        ICategoryLookup categories,
        IParticipationRecordWriter writer,
        IReadOnlyList<ParticipationAcceptanceRule> rules,
        IParticipationHistory history,
        IClock clock)
    {
        _students = students ?? throw new ArgumentNullException(nameof(students));
        _categories = categories ?? throw new ArgumentNullException(nameof(categories));
        _writer= writer?? throw new ArgumentNullException(nameof(writer));
        _history= history ?? throw new ArgumentNullException(nameof(history));
        _rules = rules ?? throw new ArgumentNullException(nameof(rules));
        _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    }

    /// <summary>Creates and stores participation after the required validation succeeds.</summary>
    public ParticipationAcceptanceResult RecordParticipation(Guid studentId,Guid categoryId,string note)
    {
        var student = _students.FindStudent(studentId);
        if (student is null)
        {
            return ParticipationAcceptanceResult.Rejected($"Student with ID '{studentId}' was not found.");
        }

        var category = _categories.FindCategory(categoryId);
        if (category is null)
        {
            return ParticipationAcceptanceResult.Rejected($"Category with ID '{categoryId}' was not found.");
        }

        var proposal = new ParticipationRecord(Guid.NewGuid(),student,category,_clock.Now,note );
        
        foreach (IParticipationAcceptanceRule rule in _rules)
        {
            ParticipationAcceptanceResult result = rule.Evaluate(proposal);
            if (!result.IsAccepted) return result;
        }
        _writer.AddRecord(proposal);
        return ParticipationAcceptanceResult.Accepted();
    }
}
