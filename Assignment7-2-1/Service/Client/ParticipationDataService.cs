using Assignment7_2_1.Domain;
using Assignment7_2_1.Contracts;
using Assignment7_2_1.Service.Client;
namespace Assignment7_2_1.Service.Client;

/// <summary>Adapts the existing repositories to the submitted client roles.</summary>
public class ParticipationDataService : IParticipationRecordingData, IParticipationRecordCorrection, IParticipationProgressReader
{
    private readonly IStudentRepository _students;
    private readonly IParticipationCategoryRepository _categories;
    private readonly IParticipationRecordRepository _records;
    public ParticipationDataService(IStudentRepository students, IParticipationCategoryRepository categories, IParticipationRecordRepository records)
    {
        _students = students ?? throw new ArgumentNullException(nameof(students));
        _categories = categories ?? throw new ArgumentNullException(nameof(categories));
        _records = records ?? throw new ArgumentNullException(nameof(records));
    }
    public List<Student> FindStudents() => new(_students.GetAll());
    public List<ParticipationCategory> FindCategories() => _categories.GetAll();
    public List<ParticipationRecord> GetHistory(Guid studentId) =>
        _records.GetAll().Where(record => record.Student.Id == studentId).ToList();
    public void StoreAccepted(ParticipationRecord record) => _records.Add(record);
    public List<ParticipationRecord> FindRecords(Guid studentId) => GetHistory(studentId);
    public void UpdateNotes(Guid id, string? notes) => _records.UpdateNotes(id, notes);
    public void Delete(Guid id) => _records.Delete(id);
    public List<ParticipationRecord> GetRecords(Guid studentId) => GetHistory(studentId);
    public int GetPointTotal(Guid studentId) => _records.GetTotalPointsForStudent(studentId);
}
