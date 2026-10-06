# Assignment 7-2-1 Reflection

Replace each placeholder with a concise response of approximately one to three
complete sentences. Cite evidence from the final implementation, such as specific
classes, interfaces, constructor parameters, method calls, guards, runtime
behavior, or substitution results.

## 1. Single Responsibility Principle

Identify two classes in the final implementation with different responsibilities.
Explain how their responsibilities give them different reasons to change.

ParticapationModifier only responsibilities change data in the ParticipationRecorder while ParticipationRecorder only store data and look up element in. This mean the ParticapationModifier will only if we want to change the way we modify the ParticipationRecorder while ParticipationRecorder only need to chagne when we want to change the way we store it

## 2. Open/Closed Principle

Explain how another participation-acceptance rule can be added without modifying
the record-creation algorithm. Cite the abstraction and rule collection used by
the implementation.

ParticipationRecorder keep List<IParticipationAcceptanceRule> which allow itself to be injected with multiple different rule.
Since IParticipationAcceptanceRule interface it can iterate through and execute the rules in its List<IParticipationAcceptanceRule> dynamically.

## 3. Liskov Substitution Principle

State the behavioral expectation shared by participation-rule implementations and
explain why the coordinator can use any implementation without rule-specific
handling.

All participation-rule implementations share the behavioral expectation defined by the abstract contract. Each rule must evaluate record and return explanation for rejection. Each rule also share the same constructor.
ParticipationRecorder interact with IParticipationAcceptanceRule rather than concrete implementations.every subclass satisfies the behavioral contract of ParticipationAcceptanceRule without altering the expected return type or side-effect guarantees can be substituted without causing unexpected behavior

## 4. Interface Segregation Principle

Identify two software clients and the focused interfaces they depend on. Name at
least one operation deliberately excluded from each client's contract.

ParticipationModifier depends on IParticipationRecordCorrection which allow ParticipationModifier to updating notes, and deleting records
StudentParticipationDashboard depends on IParticipationProgressReader, which supplies student records and point totals.


## 5. Dependency Inversion and Constructor Injection

Identify one high-level class, one low-level implementation, and the abstraction
between them. Explain how the constructor makes that dependency explicit.

The high-level ParticipationRecorder depends on IParticipationRecordingData, which the low-level ParticipationDataService implements
ParticipationDataService need IParticipationRecordRepository,IParticipationCategoryRepository and  IStudentRepository.
The recorder constructs  require no concrete object.


## 6. Composition Root and Substitution Evidence

Identify where concrete implementations are created and connected. Describe one
implementation you substituted and explain what did not have to change as a
result.

concrete implementations are created and connected in Program.cs which initialize the repositories, shared ParticipationDataService, four acceptance rules, SystemClock, ParticipationRecorder, and the clients
SystemClock can be substituted which shared the same interface so  it did  require any change in ParticipationRecorder 
