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

            return true;
        }
    }
}
