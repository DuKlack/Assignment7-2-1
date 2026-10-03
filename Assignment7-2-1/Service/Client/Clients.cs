using Assignment7_2_1.Domain;
namespace Assignment7_2_1.Service.Client;

/// <summary>Provides lookup, history, and accepted-record storage for recording.</summary>
public interface IParticipationRecordingData
{
    /// <summary>Gets students available for recording lookup.</summary>
    List<Student> FindStudents();
    /// <summary>Gets categories available for recording lookup.</summary>
    List<ParticipationCategory> FindCategories();
    /// <summary>Gets the relevant history for the supplied student.</summary>
    List<ParticipationRecord> GetHistory(Guid studentId);
    /// <summary>Stores a record after the caller has evaluated all acceptance rules.</summary>
    void StoreAccepted(ParticipationRecord record);
}
/// <summary>Provides record correction without recording or reporting operations.</summary>
public interface IParticipationRecordCorrection
{
    /// <summary>Finds the student's records for correction.</summary>
    List<ParticipationRecord> FindRecords(Guid studentId);
    /// <summary>Updates or clears the notes on an existing record.</summary>
    void UpdateNotes(Guid id, string? notes);
    /// <summary>Deletes an incorrect record.</summary>
    void Delete(Guid id);
}
/// <summary>Provides read-only student participation progress.</summary>
public interface IParticipationProgressReader
{
    /// <summary>Gets the student's records for progress display.</summary>
    List<ParticipationRecord> GetRecords(Guid studentId);
    /// <summary>Gets the point total represented by the student's current records.</summary>
    int GetPointTotal(Guid studentId);
}
