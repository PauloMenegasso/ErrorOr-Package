using ErrorOr;

namespace BuberBreakfast.ServiceErrors;

public static class Errors
{
    public static class Breakfast
    {
        public static Error InvalidName => Error.Validation(
            code: "Breakfast.InvalidName",
            description: $"Name must have between {Models.Breakfast.MAX_NAME_LENGTH} and {Models.Breakfast.MIN_NAME_LENGTH} characters"
        );

        public static Error InvalidDescription => Error.Validation(
            code: "Breakfast.InvalidDescription",
            description: $"Description must have between {Models.Breakfast.MAX_DESCRIPTION_LENGTH} and {Models.Breakfast.MIN_DESCRIPTION_LENGTH} characters"
        );

        public static Error InvalidDateTimes => Error.Validation(
            code: "Breakfast.InvalidDateTimes",
            description: "End DateTime must be greater than Start DateTime"
        );

        public static Error NotFound => Error.NotFound(
            code: "Breakfast.NotFound",
            description: "Breakfast not found"
        );
    }
}