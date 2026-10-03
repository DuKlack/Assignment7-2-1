using Assignment7_2_1.Contracts;
using Assignment7_2_1.Domain;

namespace Assignment7_2_1.Service.Client;

public class ParticipationDashboard
{
    private readonly IProgressReporting _reporting;

    public  ParticipationDashboard(IProgressReporting reporting)
    {
        _reporting = reporting;
    }

    public int GetTotalPoints(Guid studentId) => 
        _reporting.CalculateStudentPoints(studentId);
}