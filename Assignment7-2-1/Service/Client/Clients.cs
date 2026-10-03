using Assignment7_2_1.Domain;

namespace Assignment7_2_1.Service.Client;


/// <summary>Provides student lookup without student management operations.</summary>
public interface IStudentLookup
{
    /// <summary>Returns the student with the supplied identifier, or null if absent.</summary>
    Student? FindStudent(Guid studentId);
}

/// <summary>Provides participation-category lookup.</summary>
public interface ICategoryLookup
{
    /// <summary>Returns the category with the supplied identifier, or null if absent.</summary>
    ParticipationCategory? FindCategory(Guid categoryId);
}

/// <summary>Provides the participation history needed for recording decisions.</summary>
public interface IParticipationHistory
{
    /// <summary>Returns participation records for the supplied student.</summary>
    IEnumerable<ParticipationRecord> GetRecordsForStudent(Guid studentId);
}

/// <summary>Provides participation-record storage.</summary>
public interface IParticipationRecordWriter
{
    /// <summary>Stores the supplied participation record.</summary>
    void AddRecord(ParticipationRecord record);
}


/// <summary>Provides record lookup, note correction, and deletion.</summary>
public interface IParticipationModifier
{
    /// <summary>Returns the record with the supplied identifier, or null if absent.</summary>
    ParticipationRecord? FindRecord(Guid recordId);
    /// <summary>Updates the notes of an existing participation record.</summary>
    void UpdateNotes(Guid recordId, string updatedNotes);
    /// <summary>Deletes the specified record and reports whether it was removed.</summary>
    bool DeleteRecord(Guid recordId);
}


/// <summary>Provides participation records and student point totals.</summary>
public interface IProgressReporting
{
    /// <summary>Returns all stored participation records.</summary>
    IEnumerable<ParticipationRecord> GetAllRecords();
    /// <summary>Sums the awarded points for the supplied student.</summary>
    int CalculateStudentPoints(Guid studentId);
}
