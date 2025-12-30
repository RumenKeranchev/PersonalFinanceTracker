namespace PersonalFinanceTracker.Server.Modules.Users.Endpoints
{
    using Application.Services;

    public static class UsersServiceRegistration
    {
        public static void RegisterUsersServices(this IServiceCollection services)
        {
            services.AddScoped<AuthService>();
            services.AddScoped<UsersService>();
        }
    }
}
