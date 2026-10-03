using Assignment7_2_1.Contracts;
using Assignment7_2_1.Domain;

namespace Assignment7_2_1.Service.Client;

/// <summary>Reads student point totals through the progress-reporting contract.</summary>
public class ParticipationDashboard
{
    private readonly IProgressReporting _reporting;

    /// <summary>Initializes ParticipationDashboard with its required collaborators.</summary>
    public  ParticipationDashboard(IProgressReporting reporting)
    {
        _reporting = reporting;
    }

    /// <summary>Returns the current point total for the supplied student.</summary>
    public int GetTotalPoints(Guid studentId) => 
        _reporting.CalculateStudentPoints(studentId);
}
