namespace PersonalFinanceTracker.Server.Middleware
{
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class UnexpectedExeptionHandler : IExceptionHandler
    {
        private readonly ILogger _logger;

        public UnexpectedExeptionHandler(ILogger<UnexpectedExeptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var pd = new ProblemDetails
            {
                Title = "An unexpected error occurred.",
                Status = StatusCodes.Status400BadRequest,
                Instance = httpContext.Request.Path
            };

            httpContext.Response.StatusCode = pd.Status.Value;
            httpContext.Response.ContentType = "application/problem+json";

            await httpContext.Response.WriteAsJsonAsync(pd, cancellationToken);

            _logger.LogError(exception, "Unexpected exception happened while calling [{url}]", httpContext.Request.Path);

            return true;
        }
    }
}
