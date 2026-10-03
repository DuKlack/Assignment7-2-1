using Assignment7_2_1.Contracts;
using Assignment7_2_1.Domain;

namespace  Assignment7_2_1.Service.Client;

/// <summary>Delegates record corrections through the correction contract.</summary>
public class ParticipationModifier
{
    private readonly IParticipationModifier _correction;

    /// <summary>Initializes ParticipationModifier with its required collaborators.</summary>
    public ParticipationModifier(IParticipationModifier correction)
    {
        _correction = correction;
    }
    

    /// <summary>Updates the notes of an existing participation record.</summary>
    public void UpdateNotes(Guid recordId, string updatedNotes) => _correction.UpdateNotes(recordId, updatedNotes);

    /// <summary>Deletes the specified record and reports whether it was removed.</summary>
    public bool DeleteRecord(Guid recordId) => _correction.DeleteRecord(recordId);
    
}
