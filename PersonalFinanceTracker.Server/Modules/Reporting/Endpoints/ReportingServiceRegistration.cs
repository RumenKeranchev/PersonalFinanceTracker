namespace PersonalFinanceTracker.Server.Modules.Reporting.Endpoints
{
    using Application;

    public static class ReportingServiceRegistration
    {
        public static void RegisterReportingServices(this IServiceCollection services) 
            => services.AddScoped<ReportingService>();
    }
}
