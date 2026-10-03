using Assignment7_2_1.Domain;

namespace Assignment7_2_1.Service.Client;


public interface IStudentLookup
{
    Student? FindStudent(Guid studentId);
}

public interface ICategoryLookup
{
    ParticipationCategory? FindCategory(Guid categoryId);
}

public interface IParticipationHistory
{
    IEnumerable<ParticipationRecord> GetRecordsForStudent(Guid studentId);
}

public interface IParticipationRecordWriter
{
    void AddRecord(ParticipationRecord record);
}


public interface IParticipationModifier
{
    ParticipationRecord? FindRecord(Guid recordId);
    void UpdateNotes(Guid recordId, string updatedNotes);
    bool DeleteRecord(Guid recordId);
}


public interface IProgressReporting
{
    IEnumerable<ParticipationRecord> GetAllRecords();
    int CalculateStudentPoints(Guid studentId);
}