namespace PersonalFinanceTracker.Server.Modules.Reporting.Endpoints
{
    using Asp.Versioning.Builder;

    public static class ReportingEndpointBuilder
    {
        public static IEndpointRouteBuilder MapReportingModule(this IEndpointRouteBuilder builder, ApiVersionSet versionSet)
        {
            var group = builder
                .MapGroup("/api/v{v:apiVersion}/reporting")
                .WithApiVersionSet(versionSet)
                .RequireAuthorization(p => p.RequireAuthenticatedUser());

            group.MapGet("/summary", () => "Reporting Summary Endpoint");
            group.MapGet("/", () => "Reporting Summary Endpoint");

            return builder;
        }
    }
}
