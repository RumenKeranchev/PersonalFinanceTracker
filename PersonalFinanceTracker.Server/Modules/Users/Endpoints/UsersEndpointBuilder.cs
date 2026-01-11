namespace PersonalFinanceTracker.Server.Modules.Users.Endpoints
{
    using Application.DTOs.Auth;
    using Application.Services;
    using Asp.Versioning.Builder;
    using Infrastructure.Requests;
    using Microsoft.AspNetCore.Mvc;

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
                .MapGet("/logout", async (AuthService service, HttpContext ctx) =>
                {
                    string? identityName = ctx.User?.Identity?.Name;
                    if (string.IsNullOrWhiteSpace(identityName) || !Guid.TryParse(identityName, out var id))
                    {
                        return Results.Unauthorized();
                    }

                    var result = await service.LogoutAsync(id);
                    return result.ToIResult();
                })
                .RequireAuthorization(p => p.RequireAuthenticatedUser());

            return builder;
        }
    }
}
