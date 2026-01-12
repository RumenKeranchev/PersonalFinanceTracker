namespace PersonalFinanceTracker.Server.Modules.Users.Application.Services
{
    using DTOs.Users;
    using Infrastructure.Requests;

    public class UsersService
    {
        public Task<Result<UserProfileDto>> GetProfile(Guid userId) => throw new NotImplementedException();

        public Task<Result> UpdateProfile(Guid userId, UpdateProfileDto dto) => throw new NotImplementedException();

        public Task<Result> ChangePassword(Guid userId, ChangePasswordDto dto) => throw new NotImplementedException();

        public Task<Result> DeleteAccount(Guid userId) => throw new NotImplementedException();
    }
}
