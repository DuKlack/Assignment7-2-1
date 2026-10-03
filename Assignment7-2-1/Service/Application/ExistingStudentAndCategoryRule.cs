using Assignment7_2_1.Domain;
using Assignment7_2_1.Contracts;
using Assignment7_2_1.Service.Client;
namespace Assignment7_2_1.Service.Application;
/// <summary>Requires the student and category to exist in the recording data.</summary>
public sealed class ExistingStudentAndCategoryRule : IParticipationAcceptanceRule
{
    private readonly IParticipationRecordingData _data;
    public ExistingStudentAndCategoryRule(IParticipationRecordingData data) =>
        _data = data ?? throw new ArgumentNullException(nameof(data));
    public ParticipationAcceptanceResult Evaluate(ParticipationRecord record)
    {
        ArgumentNullException.ThrowIfNull(record);
        if (!_data.FindStudents().Any(student => student.Id == record.Student.Id))
            return ParticipationAcceptanceResult.Rejected("The student does not exist.");
        if (!_data.FindCategories().Any(category => category.Id == record.Category.Id))
            return ParticipationAcceptanceResult.Rejected("The participation category does not exist.");
        return ParticipationAcceptanceResult.Accepted();
    }
}
