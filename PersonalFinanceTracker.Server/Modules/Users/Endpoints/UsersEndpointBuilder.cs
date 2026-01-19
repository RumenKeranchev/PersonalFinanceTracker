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
            static CookieOptions CookieOptions(DateTime expires) => new()
            {
                HttpOnly = true,
                Expires = expires,
                SameSite = SameSiteMode.Strict,
                Secure = true
            };

            var group = builder.MapGroup("/api/auth")
                .WithApiVersionSet(versionSet)
                .AddEndpointFilter(async (ctx, next) =>
                {
                    if (!ctx.HttpContext.Request.Headers.TryGetValue("Client-Type", out var value))
                    {
                        return Results.Unauthorized();
                    }

                    if (!value.ToString().Equals(ClientType.Native.ToString(), StringComparison.InvariantCultureIgnoreCase)
                        && !value.ToString().Equals(ClientType.Browser.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        return Results.BadRequest("Invalid Client-Type!");
                    }

                    ctx.HttpContext.Items["Client-Type"] = Enum.Parse<ClientType>(value.ToString(), true);

                    return await next(ctx);
                })
                .ProducesValidationProblem();

            group.MapPost("/register", async (AuthService service, HttpContext ctx, RegisterDto model) =>
            {
                var result = await service.RegisterAsync(model);

                if (result.IsSuccess && ctx.Items.IsClientType(ClientType.Browser))
                {
                    ctx.Response.Cookies.Append("AccessToken", result.Value!.Token, CookieOptions(result.Value!.TokenExpiration));
                    ctx.Response.Cookies.Append("RefreshToken", result.Value!.RefreshToken, CookieOptions(result.Value!.RefreshTokenExpiration));

                    return Results.NoContent();
                }

                return result.ToIResult();
            });

            group.MapPost("/login", async (AuthService service, HttpContext ctx, LoginDto model) =>
            {
                var result = await service.LoginAsync(model);

                if (result.IsSuccess && ctx.Items.IsClientType(ClientType.Browser))
                {
                    ctx.Response.Cookies.Append("AccessToken", result.Value!.Token, CookieOptions(result.Value!.TokenExpiration));
                    ctx.Response.Cookies.Append("RefreshToken", result.Value!.RefreshToken, CookieOptions(result.Value!.RefreshTokenExpiration));

                    return Results.NoContent();
                }

                return result.ToIResult();
            });

            group.MapPost("/refresh", async (AuthService service, HttpContext ctx, RefreshTokenDto dto) =>
            {
                string? token = GetTokenFromRequest(ctx, dto);

                if (string.IsNullOrWhiteSpace(token))
                {
                    return Results.Unauthorized();
                }

                var result = await service.RefreshAsync(token);

                if (result.IsSuccess && ctx.Items.IsClientType(ClientType.Browser))
                {
                    ctx.Response.Cookies.Append("AccessToken", result.Value!.Token, CookieOptions(result.Value!.TokenExpiration));
                    ctx.Response.Cookies.Append("RefreshToken", result.Value!.RefreshToken, CookieOptions(result.Value!.RefreshTokenExpiration));

                    return Results.NoContent();
                }

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

                    string? token = GetTokenFromRequest(ctx, dto);

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

        private static string? GetTokenFromRequest(HttpContext ctx, RefreshTokenDto dto)
        {
            string? token = null;

            if (ctx.Items.IsClientType(ClientType.Native))
            {
                token = dto?.Token;
            }
            else if (ctx.Items.IsClientType(ClientType.Browser))
            {
                token = ctx.Request.Cookies.FirstOrDefault(c => c.Key == "refreshToken").Value;
            }

            return token;
        }
    }
}
