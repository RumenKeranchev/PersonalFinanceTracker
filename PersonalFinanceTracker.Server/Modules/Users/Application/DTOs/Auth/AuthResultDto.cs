namespace PersonalFinanceTracker.Server.Modules.Users.Application.DTOs.Auth
{
    public record AuthResultDto(string Token, DateTime TokenExpiration, string RefreshToken, DateTime RefreshTokenExpiration, string Username);
}
