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

        // Composition root: one adapter supplies the three focused client roles.
        ParticipationDataService dataService = new(studentRepository, categoryRepository, recordRepository);
        IParticipationRecordingData data = dataService;
        IParticipationRecordCorrection correction = dataService;
        IParticipationProgressReader progress = dataService;
        IClock clock = new SystemClock();
        List<IParticipationAcceptanceRule> rules = new()
        {
            new ExistingStudentAndCategoryRule(data),
            new ActiveStudentRule(),
            new DuplicateParticipationRule(data),
            new DailyParticipationLimitRule(data)
        };

        ParticipationRecorder participationService = new(data, clock, rules);
        ParticipationModifier correctionTool = new(correction);
        StudentParticipationDashboard dashboard = new(progress);
        
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

        ParticipationRecord firstRecord = correctionTool.FindRecords(maya.Id)
            .Single(record => record.Student.Id == maya.Id && record.Category.Id == askingQuestions.Id);
        ParticipationRecord secondRecord = correctionTool.FindRecords(jordan.Id)
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
        foreach (ParticipationRecord record in dashboard.GetRecords(maya.Id).Concat(dashboard.GetRecords(jordan.Id)))
        {
            Console.WriteLine(record);
        }

        Console.WriteLine("\nUPDATE");
        correctionTool.UpdateNotes(secondRecord.Id, "Explained the difference between a class and an object.");
        studentRepository.UpdateName(jordan.Id, "Jordan Lee");
        categoryRepository.UpdateDescription(
            helpingOthers.Id,
            "Offer a useful explanation or debugging hint to another student.");
        Console.WriteLine(recordRepository.GetById(secondRecord.Id));

        Console.WriteLine("\nDELETE");
        correctionTool.DeleteRecord(firstRecord.Id);
        foreach (ParticipationRecord record in dashboard.GetRecords(maya.Id).Concat(dashboard.GetRecords(jordan.Id)))
        {
            Console.WriteLine(record);
        }
        Console.WriteLine(
            $"Maya's calculated point total after deleting her record: " +
            $"{dashboard.GetPointTotal(maya.Id)}");

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
            $"{dashboard.GetPointTotal(jordan.Id)}");
        helpingOthers.PointPolicy.Points = 0;
        Console.WriteLine($"Policy points after a second direct change: {helpingOthers.PointPolicy.Points}");

        Console.WriteLine("\nINVESTIGATION SCENARIO B");
        List<Student> retrievedStudents = studentRepository.GetAll();
        retrievedStudents.Clear();
        Console.WriteLine($"Repository count after clearing the retrieved list: {studentRepository.GetAll().Count}");

        DemonstrateFixedClockSubstitution();

    }

    /// <summary>Substitutes a fixed clock at the composition root to verify the cooldown boundary.</summary>
    private static void DemonstrateFixedClockSubstitution()
    {
        Console.WriteLine("\nFIXED-CLOCK SUBSTITUTION");
        IStudentRepository students = new StudentRepository();
        IParticipationCategoryRepository categories = new ParticipationCategoryRepository();
        IParticipationRecordRepository records = new ParticipationRecordRepository();
        IParticipationRecordingData data = new ParticipationDataService(students, categories, records);
        Student student = new(Guid.NewGuid(), "Fixed-Time Student", "fixed@example.edu");
        ParticipationCategory category = new(Guid.NewGuid(), "Question", "Ask a relevant question.",
            ParticipationType.AskQuestion, new PointPolicy(2, "Clarifies course material."));
        students.Add(student);
        categories.Add(category);
        List<IParticipationAcceptanceRule> rules = new()
        {
            new ExistingStudentAndCategoryRule(data),
            new ActiveStudentRule(),
            new DuplicateParticipationRule(data),
            new DailyParticipationLimitRule(data)
        };

        DateTime start = new(2026, 10, 3, 12, 0, 0);
        DateTime[] times = { start, start.AddMinutes(9).AddSeconds(59), start.AddMinutes(10) };
        bool[] expectedAcceptance = { true, false, true };
        for (int index = 0; index < times.Length; index++)
        {
            // Only the supplied clock changes; the recorder and rule algorithm stay the same.
            IClock clock = new FixedClock(times[index]);
            ParticipationRecorder recorder = new(data, clock, rules);
            int before = records.GetAll().Count;
            ParticipationAcceptanceResult result = recorder.RecordParticipation(student.Id, category.Id, null);
            int after = records.GetAll().Count;
            Console.WriteLine($"{clock.Now:yyyy-MM-dd HH:mm:ss} | Accepted: {result.IsAccepted} | Records: {after}");
            if (!result.IsAccepted)
            {
                Console.WriteLine($"Reason: {result.RejectionReason}");
                Console.WriteLine($"Record count unchanged: {before == after}");
            }
            if (result.IsAccepted != expectedAcceptance[index] ||
                after != before + (result.IsAccepted ? 1 : 0) ||
                (!result.IsAccepted && string.IsNullOrWhiteSpace(result.RejectionReason)) ||
                (result.IsAccepted && records.GetAll().Last().OccurredAt != clock.Now))
            {
                throw new InvalidOperationException("The fixed-clock cooldown scenario did not match the expected result.");
            }
        }
        Console.WriteLine("Fixed-clock verification passed: rejected at 9:59; accepted at exactly 10:00.");
    }
}
