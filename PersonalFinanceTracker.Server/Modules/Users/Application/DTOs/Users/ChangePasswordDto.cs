namespace PersonalFinanceTracker.Server.Modules.Users.Application.DTOs.Users
{
    public record ChangePasswordDto(string CurrentPassword, string NewPassword, string ConfirmNewPassword);
}
