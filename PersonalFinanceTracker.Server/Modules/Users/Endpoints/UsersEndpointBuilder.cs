namespace PersonalFinanceTracker.Server.Modules.Users.Endpoints
{
    public static class UsersEndpointBuilder
    {
        public static IEndpointRouteBuilder MapUsersModule(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/api/users/summary", () => "Users Summary Endpoint");
            builder.MapGet("/api/users", () => "Users Summary Endpoint");

            return builder;
        }
    }
}
