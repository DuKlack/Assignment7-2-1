using Assignment7_2_1.Contracts;
using Assignment7_2_1.Domain;

namespace  Assignment7_2_1.Service.Client;

public class ParticipationModifier
{
    private readonly IParticipationModifier _correction;

    public ParticipationModifier(IParticipationModifier correction)
    {
        _correction = correction;
    }
    

    public void UpdateNotes(Guid recordId, string updatedNotes) => _correction.UpdateNotes(recordId, updatedNotes);

    public bool DeleteRecord(Guid recordId) => _correction.DeleteRecord(recordId);
    
}