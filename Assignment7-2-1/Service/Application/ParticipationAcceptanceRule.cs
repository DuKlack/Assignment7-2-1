using Assignment7_2_1.Contracts;
using Assignment7_2_1.Domain;

namespace Assignment7_2_1.Service.Application;

/// <summary>Provides shared identity and metadata for participation acceptance rules.</summary>
public abstract class ParticipationAcceptanceRule : IParticipationAcceptanceRule
{
    /// <summary>Initializes ParticipationAcceptanceRule with its required collaborators.</summary>
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
    /// <summary>Gets or sets the display name of this rule.</summary>
    public string Name { get; set; }
    /// <summary>Gets or sets the instructor associated with this rule.</summary>
    public string Instructor { get; set; }
    /// <summary>Gets or sets the identifier of this rule.</summary>
    public Guid Id { get; set; }

    /// <summary>Returns the rule type name for display.</summary>
    public virtual string GetRuleType()
    {
        return GetType().Name;
    }

    /// <summary>Evaluates the proposed record without storing it and explains any rejection.</summary>
    public abstract ParticipationAcceptanceResult Evaluate(ParticipationRecord record);
}
