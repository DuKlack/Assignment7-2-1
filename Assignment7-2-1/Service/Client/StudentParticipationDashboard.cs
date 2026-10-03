using Assignment7_2_1.Domain;
namespace Assignment7_2_1.Service.Client;
/// <summary>Reads participation records and totals through the progress role.</summary>
public class StudentParticipationDashboard
{
    private readonly IParticipationProgressReader _progress;
    public StudentParticipationDashboard(IParticipationProgressReader progress) =>
        _progress = progress ?? throw new ArgumentNullException(nameof(progress));
    public List<ParticipationRecord> GetRecords(Guid studentId) => _progress.GetRecords(studentId);
    public int GetPointTotal(Guid studentId) => _progress.GetPointTotal(studentId);
}
