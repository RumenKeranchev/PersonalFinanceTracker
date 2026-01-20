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
        public static Result Failure(Error error) => new(false, error ?? throw new ApplicationException(nameof(error)));

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
                return Results.NoContent();
            }
            else
            {
                if (result.Error is ValidationError error)
                {
                    return Results.ValidationProblem(error.Errors);
                }
                else
                {
                    return Results.Problem(statusCode: (int)result.Error!.StatusCode, title: result.Error.Code, detail: result.Error.Message);
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "<Pending>")]
        public static IResult ToIResult<T>(this Result<T> result)
        {
            if (result.IsSuccess)
            {
                return Results.Ok(result.Value);
            }
            else
            {
                if (result.Error is ValidationError error)
                {
                    return Results.ValidationProblem(error.Errors);
                }
                else
                {
                    return Results.Problem(statusCode: (int)result.Error!.StatusCode, title: result.Error.Code, detail: result.Error.Message);
                }
            }
        }
    }
}
