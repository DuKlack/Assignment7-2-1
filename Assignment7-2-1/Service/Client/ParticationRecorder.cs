using Assignment7_2_1.Contracts;
using Assignment7_2_1.Domain;

namespace  Assignment7_2_1.Service.Client;


public class ParticipationRecorder
{
    private readonly IStudentLookup _studentLookup;
    private readonly ICategoryLookup _categoryLookup;
    private readonly IParticipationHistory _history;
    private readonly IParticipationModifier _correction;
    private readonly IParticipationRecordWriter _recordWriter; 


    public ParticipationRecorder(
        IStudentLookup studentLookup,
        ICategoryLookup categoryLookup,
        IParticipationHistory history,
        IParticipationModifier correction,
        IParticipationRecordWriter recordWriter
        )
    {
        _studentLookup = studentLookup;
        _categoryLookup = categoryLookup;
        _history = history;
        _correction = correction;
        _recordWriter = recordWriter;

    }
    public void RecordParticipation(Guid recordId,Guid studentId, Guid categoryId,DateTime time, string notes)
    {
        var student = _studentLookup.FindStudent(studentId) 
                      ?? throw new KeyNotFoundException($"Student {studentId} not found.");
            
        var category = _categoryLookup.FindCategory(categoryId) 
                       ?? throw new KeyNotFoundException($"Category {categoryId} not found.");

        var record = new ParticipationRecord(recordId ,student, category,time, notes);
        
        _recordWriter.AddRecord(record);
    }
}
    
