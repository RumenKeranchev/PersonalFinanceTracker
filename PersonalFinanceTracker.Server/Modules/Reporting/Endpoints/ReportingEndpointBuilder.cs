namespace PersonalFinanceTracker.Server.Modules.Reporting.Endpoints
{
    using Application;
    using Application.DTOs;
    using Asp.Versioning.Builder;
    using Infrastructure.Requests;

    public static class ReportingEndpointBuilder
    {
        public static IEndpointRouteBuilder MapReportingModule(this IEndpointRouteBuilder builder, ApiVersionSet versionSet)
        {
            var group = builder
                .MapGroup("/api/v{v:apiVersion}/reporting")
                .WithApiVersionSet(versionSet)
                .RequireAuthorization(p => p.RequireAuthenticatedUser());

            group
                .MapGet("/", async (ReportingService service) =>
                {
                    var result = await service.GetDashboardAsync();
                    return result.ToIResult();
                })
                .Produces<Dashboard>();

            return builder;
        }
    }
}
