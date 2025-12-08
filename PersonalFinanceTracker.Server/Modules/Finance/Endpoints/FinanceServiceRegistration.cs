namespace PersonalFinanceTracker.Server.Modules.Finance.Endpoints
{
    using Application;

    public static class FinanceServiceRegistration
    {
        public static void RegisterFinanceServices(this IServiceCollection services)
        {            
            services.AddScoped<BudgetService>();
            services.AddScoped<CategoryService>();
            services.AddScoped<TransactionService>();
        }
    }
}
