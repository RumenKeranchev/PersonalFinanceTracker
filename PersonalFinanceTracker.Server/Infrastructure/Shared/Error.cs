namespace PersonalFinanceTracker.Server.Infrastructure.Shared
{
    using FluentValidation.Results;

    public record Error(string Code, string Message)
    {
        public static Error InvalidId => new("common.invalid_id", "Provided id is invalid.");
    }

    public sealed record ValidationError(string Code, string Message, IReadOnlyDictionary<string, string[]> Errors) : Error(Code, Message);

    public static class ErrorsExtensions
    {
        public static ValidationError ToValidationError(this ValidationResult result)
        {
            var errors = result.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            return new ValidationError(
                "validation.failed",
                "One or more validation errors occurred.",
                errors
            );
        }
    }
}
