namespace PersonalFinanceTracker.Server.Modules.Reporting.Endpoints
{
    public static class ReportingEndpointBuilder
    {
        public static IEndpointRouteBuilder MapReportingModule(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/api/reporting/summary", () => "Reporting Summary Endpoint");
            builder.MapGet("/api/reporting", () => "Reporting Summary Endpoint");

            return builder;
        }
    }
}
