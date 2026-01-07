namespace PersonalFinanceTracker.Server.Modules.Users.Application.DTOs.Auth
{
    public record AuthResultDto(string Token, DateTime ExpirationDate);
}
