using Assignment7_2_1.Contracts;
using Assignment7_2_1.Domain;
using Assignment7_2_1.Service;
using Assignment7_2_1.Service.Application;
using Assignment7_2_1.Service.Client;
using Assignment7_2_1.Service.Clock;

namespace Assignment7_2_1;

public static class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("STUDENT PARTICIPATION MANAGEMENT SYSTEM");
        Console.WriteLine(new string('=', 40));

        IStudentRepository studentRepository = new StudentRepository();
        IParticipationCategoryRepository categoryRepository = new ParticipationCategoryRepository();
        IParticipationRecordRepository recordRepository = new ParticipationRecordRepository();

        IClock clock = new SystemClock();
        IParticipationRecordingData data = new ParticipationDataService(studentRepository, categoryRepository, recordRepository);
        List<IParticipationAcceptanceRule> rules = new()
        {
            new ExistingStudentAndCategoryRule(data),
            new ActiveStudentRule(),
            new DuplicateParticipationRule(data),
            new DailyParticipationLimitRule(data)
        };

        ParticipationRecorder participationService = new(data, clock, rules);
        
        Student maya = new(Guid.NewGuid(), "Maya Chen", "maya@example.edu");
        Student jordan = new(Guid.NewGuid(), "Jordan Smith", "jordan@example.edu");

        ParticipationCategory askingQuestions = new(
            Guid.NewGuid(),
            "Ask a Question",
            "Ask a relevant question that helps clarify course material.",
            ParticipationType.AskQuestion,
            new PointPolicy(2, "Questions can improve understanding for the whole class."));

        ParticipationCategory helpingOthers = new(
            Guid.NewGuid(),
            "Help Others",
            "Provide constructive assistance without completing another student's work.",
            ParticipationType.HelpOthers,
            new PointPolicy(4, "Peer explanations require preparation and communication."));

        askingQuestions.Examples.Add("Asked why a constructor should guard its parameters.");
        helpingOthers.Examples.Add("Helped a teammate trace a CRUD update.");

        Console.WriteLine("\nCREATE");
        studentRepository.Add(maya);
        studentRepository.Add(jordan);
        categoryRepository.Add(askingQuestions);
        categoryRepository.Add(helpingOthers);

        ParticipationAcceptanceResult firstResult =
            participationService.RecordParticipation(
                maya.Id,
                askingQuestions.Id,
                "Connected the question to class invariants.");

        ParticipationAcceptanceResult secondResult =
            participationService.RecordParticipation(
                jordan.Id,
                helpingOthers.Id,
                null);

        Console.WriteLine($"Maya's record accepted: {firstResult.IsAccepted}");
        Console.WriteLine($"Jordan's record accepted: {secondResult.IsAccepted}");

        ParticipationRecord firstRecord = recordRepository.GetAll()
            .Single(record => record.Student.Id == maya.Id && record.Category.Id == askingQuestions.Id);
        ParticipationRecord secondRecord = recordRepository.GetAll()
            .Single(record => record.Student.Id == jordan.Id && record.Category.Id == helpingOthers.Id);
        
        Console.WriteLine($"Created {recordRepository.GetAll().Count} participation records.");

        Console.WriteLine("\nREJECTED DUPLICATE PARTICIPATION");
        int recordCountBeforeRejection = recordRepository.GetAll().Count;
        ParticipationAcceptanceResult duplicateResult = participationService.RecordParticipation(
            maya.Id,
            askingQuestions.Id,
            "A duplicate participation attempt.");
        Console.WriteLine($"Accepted: {duplicateResult.IsAccepted}");
        Console.WriteLine($"Reason: {duplicateResult.RejectionReason}");
        Console.WriteLine($"Record count unchanged: {recordCountBeforeRejection == recordRepository.GetAll().Count}");

        Console.WriteLine("\nREAD ONE");
        Console.WriteLine(recordRepository.GetById(firstRecord.Id));

        Console.WriteLine("\nREAD ALL");
        foreach (ParticipationRecord record in recordRepository.GetAll())
        {
            Console.WriteLine(record);
        }

        Console.WriteLine("\nUPDATE");
        recordRepository.UpdateNotes(secondRecord.Id, "Explained the difference between a class and an object.");
        studentRepository.UpdateName(jordan.Id, "Jordan Lee");
        categoryRepository.UpdateDescription(
            helpingOthers.Id,
            "Offer a useful explanation or debugging hint to another student.");
        Console.WriteLine(recordRepository.GetById(secondRecord.Id));

        Console.WriteLine("\nDELETE");
        recordRepository.Delete(firstRecord.Id);
        foreach (ParticipationRecord record in recordRepository.GetAll())
        {
            Console.WriteLine(record);
        }
        Console.WriteLine(
            $"Maya's calculated point total after deleting her record: " +
            $"{recordRepository.GetTotalPointsForStudent(maya.Id)}");

        Console.WriteLine("\nREJECTED DOMAIN OPERATION");
        try
        {
            _ = new ParticipationRecord(
                Guid.NewGuid(),
                maya,
                askingQuestions,
                clock.Now.AddDays(1),
                null,
                clock.Now);
        }
        catch (ArgumentOutOfRangeException exception)
        {
            Console.WriteLine($"Expected exception: {exception.Message}");
        }
        Console.WriteLine($"Record count remains: {recordRepository.GetAll().Count}");

        Console.WriteLine("\nREJECTED REPOSITORY OPERATION");
        try
        {
            studentRepository.Add(maya);
        }
        catch (InvalidOperationException exception)
        {
            Console.WriteLine($"Expected exception: {exception.Message}");
        }
        Console.WriteLine($"Student count remains: {studentRepository.GetAll().Count}");

        Console.WriteLine("\nINVESTIGATION SCENARIO A");
        int pointsShownBeforePolicyChange = secondRecord.AwardedPoints;
        helpingOthers.PointPolicy.Points = 20;
        Console.WriteLine($"Record points before policy change: {pointsShownBeforePolicyChange}");
        Console.WriteLine($"Same record points after policy change: {secondRecord.AwardedPoints}");
        Console.WriteLine(
            $"Jordan's calculated total after policy change: " +
            $"{recordRepository.GetTotalPointsForStudent(jordan.Id)}");
        helpingOthers.PointPolicy.Points = 0;
        Console.WriteLine($"Policy points after a second direct change: {helpingOthers.PointPolicy.Points}");

        Console.WriteLine("\nINVESTIGATION SCENARIO B");
        List<Student> retrievedStudents = studentRepository.GetAll();
        retrievedStudents.Clear();
        Console.WriteLine($"Repository count after clearing the retrieved list: {studentRepository.GetAll().Count}");

    }
}
