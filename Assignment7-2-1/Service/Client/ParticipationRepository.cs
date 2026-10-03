using Assignment7_2_1.Contracts;
using Assignment7_2_1.Domain;

namespace Assignment7_2_1.Service.Client;

public class ParticipationRepository : 
    IStudentLookup, 
    ICategoryLookup, 
    IParticipationHistory, 
    IParticipationRecordWriter, 
    IParticipationModifier, 
    IProgressReporting
{
    private readonly List<ParticipationCategory> _categories = new();
    private readonly List<Student> _students = new();
    private readonly List<ParticipationRecord> _participationRecords = new();

    // IStudentLookup
    public Student? FindStudent(Guid studentId) => 
        _students.Find(s => s.Id == studentId);

    // ICategoryLookup
    public ParticipationCategory? FindCategory(Guid categoryId) => 
        _categories.Find(c => c.Id == categoryId);

    // IParticipationHistory
    public IEnumerable<ParticipationRecord> GetRecordsForStudent(Guid studentId) => 
        _participationRecords.Where(r => r.Student.Id == studentId);

    // IParticipationRecordWriter
    public void AddRecord(ParticipationRecord record) => 
        _participationRecords.Add(record);

    // IParticipationModifier
    public ParticipationRecord? FindRecord(Guid recordId) => 
        _participationRecords.Find(r => r.Id == recordId);

    public void UpdateNotes(Guid recordId, string updatedNotes)
    {
        var record = FindRecord(recordId);
        record?.UpdateNotes(updatedNotes);
    }

    public bool DeleteRecord(Guid recordId)
    {
        var record = FindRecord(recordId);
        return record is not null && _participationRecords.Remove(record);
    }

    // IProgressReporting
    public IEnumerable<ParticipationRecord> GetAllRecords() => 
        _participationRecords.AsReadOnly();

    public int CalculateStudentPoints(Guid studentId) => 
        _participationRecords
            .Where(r => r.Student.Id == studentId)
            .Sum(r => r.AwardedPoints);
}