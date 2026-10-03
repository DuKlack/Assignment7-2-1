using Assignment7_2_1.Contracts;
using Assignment7_2_1.Domain;

namespace Assignment7_2_1.Service.Client;

/// <summary>Stores participation data in memory and implements focused client roles.</summary>
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
    /// <summary>Returns the student with the supplied identifier, or null if absent.</summary>
    public Student? FindStudent(Guid studentId) => 
        _students.Find(s => s.Id == studentId);

    // ICategoryLookup
    /// <summary>Returns the category with the supplied identifier, or null if absent.</summary>
    public ParticipationCategory? FindCategory(Guid categoryId) => 
        _categories.Find(c => c.Id == categoryId);

    // IParticipationHistory
    /// <summary>Returns participation records for the supplied student.</summary>
    public IEnumerable<ParticipationRecord> GetRecordsForStudent(Guid studentId) => 
        _participationRecords.Where(r => r.Student.Id == studentId);

    // IParticipationRecordWriter
    /// <summary>Stores the supplied participation record.</summary>
    public void AddRecord(ParticipationRecord record) => 
        _participationRecords.Add(record);

    // IParticipationModifier
    /// <summary>Returns the record with the supplied identifier, or null if absent.</summary>
    public ParticipationRecord? FindRecord(Guid recordId) => 
        _participationRecords.Find(r => r.Id == recordId);

    /// <summary>Updates the notes of an existing participation record.</summary>
    public void UpdateNotes(Guid recordId, string updatedNotes)
    {
        var record = FindRecord(recordId);
        record?.UpdateNotes(updatedNotes);
    }

    /// <summary>Deletes the specified record and reports whether it was removed.</summary>
    public bool DeleteRecord(Guid recordId)
    {
        var record = FindRecord(recordId);
        return record is not null && _participationRecords.Remove(record);
    }

    // IProgressReporting
    /// <summary>Returns all stored participation records.</summary>
    public IEnumerable<ParticipationRecord> GetAllRecords() => 
        _participationRecords.AsReadOnly();

    /// <summary>Sums the awarded points for the supplied student.</summary>
    public int CalculateStudentPoints(Guid studentId) => 
        _participationRecords
            .Where(r => r.Student.Id == studentId)
            .Sum(r => r.AwardedPoints);
}
