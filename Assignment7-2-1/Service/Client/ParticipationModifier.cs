using Assignment7_2_1.Domain;
namespace Assignment7_2_1.Service.Client;
/// <summary>Finds and corrects existing records through the correction role.</summary>
public class ParticipationModifier
{
    private readonly IParticipationRecordCorrection _records;
    public ParticipationModifier(IParticipationRecordCorrection records) =>
        _records = records ?? throw new ArgumentNullException(nameof(records));
    public List<ParticipationRecord> FindRecords(Guid studentId) => _records.FindRecords(studentId);
    public void UpdateNotes(Guid id, string? notes) => _records.UpdateNotes(id, notes);
    public void DeleteRecord(Guid id) => _records.Delete(id);
}
