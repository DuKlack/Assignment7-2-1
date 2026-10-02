using Assignment7_2_1.Contracts;
using Assignment7_2_1.Domain;

namespace Assignment7_2_1.Service.Application;

public class EligibilityRule : ParticipationAcceptanceRule
{
    private readonly IStudentRepository _studentRepository;
    private readonly IParticipationCategoryRepository _categoryRepository;
    public EligibilityRule(Guid id, string name, string instructor, IStudentRepository studentRepository, IParticipationCategoryRepository categoryRepository) : base(name, instructor, id)
    {
        _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    }
    public override string GetRuleType() => nameof(EligibilityRule);
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
