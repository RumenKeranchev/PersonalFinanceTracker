namespace PersonalFinanceTracker.Server.Modules.Users.Endpoints
{
    using Application;
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

            group.MapPost("/refresh", async (AuthService service, HttpRequest request, RefreshTokenDto dto) =>
            {
                string? token = request.Cookies.FirstOrDefault(c => c.Key == "refreshToken").Value;

                if (string.IsNullOrWhiteSpace(token))
                {
                    token = dto?.Token;
                }

                if (string.IsNullOrWhiteSpace(token))
                {
                    return Results.Unauthorized();
                }

                var result = await service.RefreshAsync(token);
                return result.ToIResult();
            });

            group
                .MapPost("/logout", async (AuthService service, HttpContext ctx, RefreshTokenDto dto) =>
                {
                    string? identityName = ctx.User?.Identity?.Name;
                    if (string.IsNullOrWhiteSpace(identityName) || !Guid.TryParse(identityName, out var id))
                    {
                        return Results.Unauthorized();
                    }

                    string? token = ctx.Request.Cookies.FirstOrDefault(c => c.Key == "refreshToken").Value;

                    if (string.IsNullOrWhiteSpace(token))
                    {
                        token = dto?.Token;
                    }

                    if (string.IsNullOrWhiteSpace(token))
                    {
                        return Results.Unauthorized();
                    }

                    var result = await service.LogoutAsync(id, token);
                    return result.ToIResult();
                })
                .RequireAuthorization(p => p.RequireAuthenticatedUser());

            group
                .MapGet("/invalidate/{userId}", async (AuthService service, HttpContext ctx, Guid userId) =>
                {
                    var result = await service.InvalidateUserAccess(userId);
                    return result.ToIResult();
                })
                .RequireAuthorization(p =>
                {
                    p.RequireAuthenticatedUser();
                    p.RequireRole(Roles.Admin);
                });

            return builder;
        }
    }
}
