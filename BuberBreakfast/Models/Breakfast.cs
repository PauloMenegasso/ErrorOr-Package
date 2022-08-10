using BuberBreakfast.ServiceErrors;
using ErrorOr;

namespace BuberBreakfast.Models;

public class Breakfast
{
    public Guid Id { get; }
    public string Name { get; }
    public string Description { get; }
    public DateTime StartDateTime { get; }
    public DateTime EndDateTime { get; }
    public DateTime LastModifiedDateTime { get; }
    public List<string> Savory { get; }
    public List<string> Sweet { get; }
    public const int MIN_NAME_LENGTH = 3;
    public const int MAX_NAME_LENGTH = 50;
    public const int MIN_DESCRIPTION_LENGTH = 30;
    public const int MAX_DESCRIPTION_LENGTH = 300;
    private Breakfast(
        Guid id,
        string name,
        string description,
        DateTime startDateTime,
        DateTime endDateTime,
        DateTime lastModifiedDateTime,
        List<string> savory,
        List<string> sweet)
    {
        Id = id;
        Name = name;
        Description = description;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        LastModifiedDateTime = lastModifiedDateTime;
        Savory = savory;
        Sweet = sweet;
    }

    public static ErrorOr<Breakfast> Create(
        string name,
        string description,
        DateTime startDateTime,
        DateTime endDateTime,
        List<string> savory,
        List<string> sweet,
        Guid? id = null
    )
    {
        var errors = new List<Error>();

        if (name.Length is < MIN_NAME_LENGTH or > MAX_NAME_LENGTH)
        {
            errors.Add(Errors.Breakfast.InvalidName);
        }

        if (description.Length is < MIN_DESCRIPTION_LENGTH or > MAX_DESCRIPTION_LENGTH)
        {
            errors.Add(Errors.Breakfast.InvalidDescription);
        }

        if (endDateTime <= startDateTime) {
            errors.Add(Errors.Breakfast.InvalidDateTimes);
        }

        return errors.Any()
            ? errors
            : new Breakfast(
                id ?? Guid.NewGuid(),
                name,
                description,
                startDateTime,
                endDateTime,
                DateTime.UtcNow,
                savory,
                sweet
            );

    }
}
