using Assignment7_2_1.Contracts;
using Assignment7_2_1.Domain;

namespace  Assignment7_2_1.Service.Client;


public class ParticipationRecorder
{
    private readonly IStudentLookup _studentLookup;
    private readonly ICategoryLookup _categoryLookup;
    private readonly IParticipationHistory _history;
    private readonly IParticipationRecordWriter _writer;

    public ParticipationRecorder(
        IStudentLookup studentLookup,
        ICategoryLookup categoryLookup,
        IParticipationHistory history,
        IParticipationRecordWriter writer)
    {
        _studentLookup = studentLookup;
        _categoryLookup = categoryLookup;
        _history = history;
        _writer = writer;
    }
    public void RecordParticipation(Guid recordId,Guid studentId, Guid categoryId,DateTime time, string notes)
    {
        var student = _studentLookup.FindStudent(studentId) 
                      ?? throw new KeyNotFoundException($"Student {studentId} not found.");
            
        var category = _categoryLookup.FindCategory(categoryId) 
                       ?? throw new KeyNotFoundException($"Category {categoryId} not found.");

        var record = new ParticipationRecord(recordId ,student, category,time, notes);
        _writer.AddRecord(record);
    }
}
    
