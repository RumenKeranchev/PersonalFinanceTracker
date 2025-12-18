namespace PersonalFinanceTracker.Server.Infrastructure.Requests
{
    using Shared;

    /// <summary>
    /// Represents the outcome of an operation, indicating success or failure and providing error details if applicable.
    /// </summary>
    /// <remarks>Use the static methods <see cref="Success"/> and <see cref="Failure(Error)"/> to create
    /// instances representing successful or failed results.
    /// <br/>
    /// <br/>
    /// Instead of this: 
    /// <code>
    /// throw new Exception("Transaction not found");
    /// </code>
    /// or this:
    /// <code>
    /// return null;
    /// </code>
    /// </remarks>
    public record Result
    {
        public bool IsSuccess { get; }
        public Error? Error { get; }

        protected Result(bool isSuccess, Error? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, null);
        public static Result Failure(Error error) => new(false, error ?? throw new ArgumentNullException(nameof(error)));

        public static implicit operator Result(Error error)
        {
            return Failure(error);
        }
    }

    public record Result<T> : Result
    {
        public T? Value { get; }

        private Result(T value) : base(true, null)
        {
            Value = value;
        }

        private Result(Error error) : base(false, error) { }

        public static implicit operator Result<T>(T value)
        {
            return new(value);
        }

        public static implicit operator Result<T>(Error error)
        {
            return new(error);
        }
    }

    public static class ResultExtensions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "<Pending>")]
        public static IResult ToIResult(this Result result)
        {
            if (result.IsSuccess)
            {
                return Results.Ok();
            }

            if (result.Error!.Code.StartsWith("validation"))
            {
                return result.Error is ValidationError error
                    ? Results.ValidationProblem(error.Errors)
                    : Results.ValidationProblem(new Dictionary<string, string[]> { { result.Error.Code, new[] { result.Error.Message } } });
            }

            return result.Error.Code switch
            {
                "not_found" => Results.NotFound(result.Error.Message),
                "unauthorized" => Results.Unauthorized(),
                "conflict" => Results.Conflict(result.Error.Message),
                _ => Results.BadRequest(result.Error.Message)
            };
        }
    }
}
