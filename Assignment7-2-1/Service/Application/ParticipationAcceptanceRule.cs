using Assignment7_2_1.Contracts;
using Assignment7_2_1.Domain;

namespace Assignment7_2_1.Service.Application;

public abstract class ParticipationAcceptanceRule : IParticipationAcceptanceRule
{
    public ParticipationAcceptanceRule(
        string name,
        string instructor,
        Guid id)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(instructor))
        {
            throw new ArgumentException("Instructor is required.", nameof(instructor));
        }

        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id is required.", nameof(id));
        }
        Name = name;
        Instructor = instructor;
        Id = id;
    }
    public string Name { get; set; }
    public string Instructor { get; set; }
    public Guid Id { get; set; }

    public virtual string GetRuleType()
    {
        return GetType().Name;
    }

    public abstract ParticipationAcceptanceResult Evaluate(ParticipationRecord record);
}
