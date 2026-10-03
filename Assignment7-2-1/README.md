# Assignment 7-2-1 Reflection

## 1. Single Responsibility Principle

`ParticipationRecorder` coordinates record creation by looking up the student and category, evaluating the configured rules, and calling `StoreAccepted` only after acceptance. `DuplicateParticipationRule` enforces the ten-minute category cooldown, so it changes when that policy changes, while the recorder changes when the overall recording workflow changes.

## 2. Open/Closed Principle

A new rule can implement `IParticipationAcceptanceRule` and be added to the `List<IParticipationAcceptanceRule>` configured in `Program.cs`. `ParticipationRecorder` copies the injected collection into its `_rules` field and evaluates every rule through `Evaluate`, so adding a rule does not require changing `RecordParticipation` or adding concrete-type checks.

## 3. Liskov Substitution Principle

Each `IParticipationAcceptanceRule` implementation evaluates a valid proposed record without storing it and returns `ParticipationAcceptanceResult`, including a clear reason when it rejects. `ActiveStudentRule`, `ExistingStudentAndCategoryRule`, `DuplicateParticipationRule`, and `DailyParticipationLimitRule` honor this contract, allowing the recorder to call `Evaluate` and check `IsAccepted` without rule-specific handling.

## 4. Interface Segregation Principle

`ParticipationModifier` depends on `IParticipationRecordCorrection`, which supports finding records, updating notes, and deleting records but excludes `StoreAccepted`. `StudentParticipationDashboard` depends on `IParticipationProgressReader`, which supplies student records and point totals but excludes both note updates and deletion. These clients receive only their required roles even though one `ParticipationDataService` implements all three role interfaces.

## 5. Dependency Inversion and Constructor Injection

The high-level `ParticipationRecorder` depends on `IParticipationRecordingData`, which the low-level `ParticipationDataService` implements by delegating to the existing repositories. Its constructor explicitly requires that data abstraction, an `IClock`, and an `IReadOnlyList<IParticipationAcceptanceRule>`, storing the supplied dependencies in private readonly fields. The recorder constructs no concrete repository or clock and obtains time through `_clock.Now`.

## 6. Composition Root and Substitution Evidence

`Program.cs` is the composition root: it creates the repositories, shared `ParticipationDataService`, four rules, `SystemClock`, recorder, correction tool, and dashboard, connecting clients through their focused interfaces. `DemonstrateFixedClockSubstitution` supplies `FixedClock` through `IClock` to the same recorder and rules, accepting the initial record, rejecting a duplicate at 9 minutes 59 seconds without changing storage, and accepting it at exactly 10 minutes. This substitution requires no change to the recorder, rule interfaces, or rule-evaluation algorithm.
