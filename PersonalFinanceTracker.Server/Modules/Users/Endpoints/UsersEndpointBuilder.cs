namespace PersonalFinanceTracker.Server.Modules.Users.Endpoints
{
    using Application.DTOs.Auth;
    using Application.Services;
    using Asp.Versioning.Builder;
    using Infrastructure.Requests;

    public static class UsersEndpointBuilder
    {
        public static IEndpointRouteBuilder MapUsersModule(this IEndpointRouteBuilder builder, ApiVersionSet versionSet)
        {
            var group = builder.MapGroup("/api/users").WithApiVersionSet(versionSet);

            group.MapPost("/register", async (AuthService service, RegisterDto model) =>
            {
                var result = await service.RegisterAsync(model);

                return result.ToIResult();
            });

            group.MapPost("/login", async (AuthService service, LoginDto model) =>
            {
                var result = await service.LoginAsync(model);
                return result.ToIResult();
            });

            return builder;
        }
    }
}
