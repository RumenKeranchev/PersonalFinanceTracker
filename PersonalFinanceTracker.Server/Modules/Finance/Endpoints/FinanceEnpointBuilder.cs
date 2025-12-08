namespace PersonalFinanceTracker.Server.Modules.Finance.Endpoints
{
    public static class ReportingEnpointBuilder
    {
        public static IEndpointRouteBuilder MapFinanceModule(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/api/finance/summary", () =>
            {
                return "Finance Summary Endpoint";
            });
            builder.MapGet("/api/finance", () => "Finance Summary Endpoint");

            return builder;
        }
    }
}
