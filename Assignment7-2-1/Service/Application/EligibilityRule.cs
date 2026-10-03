using Assignment7_2_1.Contracts;
using Assignment7_2_1.Domain;

namespace Assignment7_2_1.Service.Application;

/// <summary>Checks student and category existence and active-student status.</summary>
public class EligibilityRule : ParticipationAcceptanceRule
{
    private readonly IStudentRepository _studentRepository;
    private readonly IParticipationCategoryRepository _categoryRepository;
    /// <summary>Initializes EligibilityRule with its required collaborators.</summary>
    public EligibilityRule(Guid id, string name, string instructor, IStudentRepository studentRepository, IParticipationCategoryRepository categoryRepository) : base(name, instructor, id)
    {
        _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }
    /// <summary>Returns the rule type name for display.</summary>
    public override string GetRuleType() => nameof(EligibilityRule);
    /// <summary>Evaluates the proposed record without storing it and explains any rejection.</summary>
    public override ParticipationAcceptanceResult Evaluate(ParticipationRecord record)
    {
        ArgumentNullException.ThrowIfNull(record);
        Student student;
        try { student = _studentRepository.GetById(record.Student.Id); }
        catch (KeyNotFoundException) { return ParticipationAcceptanceResult.Rejected("The student does not exist."); }
        try { _ = _categoryRepository.GetById(record.Category.Id); }
        catch (KeyNotFoundException) { return ParticipationAcceptanceResult.Rejected("The participation category does not exist."); }
        return student.IsActive ? ParticipationAcceptanceResult.Accepted() : ParticipationAcceptanceResult.Rejected("The student is inactive.");
    }
}
