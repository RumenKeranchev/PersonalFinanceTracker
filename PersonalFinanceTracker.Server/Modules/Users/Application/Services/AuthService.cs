namespace PersonalFinanceTracker.Server.Modules.Users.Application.Services
{
    using Domain;
    using DTOs.Auth;
    using Infrastructure.Requests;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Resourses;
    using System.Net;
    using Validators.Auth;

    public class AuthService
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger _logger;
        private readonly TokenGenerator _tokenGenerator;
        private readonly AppDbContext _db;
        private readonly int _refreshTokenExpirationDays;

        public AuthService(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ILogger<AuthService> logger, TokenGenerator tokenGenerator, AppDbContext db,
            IConfiguration config)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _tokenGenerator = tokenGenerator;
            _db = db;

            string refreshTokenExpiration = config["Jwt:RefreshTokenExpirationInDays"] ?? throw new InvalidOperationException("JWT Refresh Token Expiration is not configured.");
            _refreshTokenExpirationDays = int.Parse(refreshTokenExpiration);
        }

        public async Task<Result<AuthResultDto>> RegisterAsync(RegisterDto model)
        {
            var validationResult = new RegisterDtoValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                return validationResult.ToValidationError();
            }

            var userExists = await _userManager.FindByEmailAsync(model.Email);

            if (userExists != null)
            {
                return UsersErrors.UserAlreadyExists;
            }

            var user = new AppUser(model.Email, model.Username);
            var identityRes = await _userManager.CreateAsync(user, model.Password);

            if (!identityRes.Succeeded)
            {
                return identityRes.ToValidationError("users.auth.creation_failed");
            }

            identityRes = await _userManager.AddToRoleAsync(user, Roles.User);

            if (!identityRes.Succeeded)
            {
                return identityRes.ToValidationError("users.auth.role_assignment_failed");
            }

            var signInRes = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);
            if (!signInRes.Succeeded)
            {
                return UsersErrors.InvalidCredentials;
            }

            (string token, var expires) = _tokenGenerator.GenerateAccessToken(user, [Roles.User]);
            string refreshToken = _tokenGenerator.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken(refreshToken, _refreshTokenExpirationDays, user.Id);

            _db.RefreshTokens.Add(refreshTokenEntity);

            await _db.SaveChangesAsync();

            _logger.LogInformation("User [{Email}] registered successfully.", model.Email);

            return new AuthResultDto(token, expires, refreshToken, refreshTokenEntity.ExpiresAt, user.UserName ?? user.Email!);
        }

        public async Task<Result<AuthResultDto>> LoginAsync(LoginDto model)
        {
            var result = new LoginDtoValidator().Validate(model);
            if (!result.IsValid)
            {
                return result.ToValidationError();
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return UsersErrors.InvalidCredentials;
            }

            var signInRes = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);
            if (!signInRes.Succeeded)
            {
                return UsersErrors.InvalidCredentials;
            }

            var roles = await _userManager.GetRolesAsync(user);

            (string token, var expires) = _tokenGenerator.GenerateAccessToken(user, roles);
            string refreshToken = _tokenGenerator.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken(refreshToken, _refreshTokenExpirationDays, user.Id);

            _db.RefreshTokens.Add(refreshTokenEntity);

            await _db.SaveChangesAsync();

            _logger.LogInformation("User [{Email}] logged in successfully.", model.Email);

            return new AuthResultDto(token, expires, refreshToken, refreshTokenEntity.ExpiresAt, user.UserName ?? user.Email!);
        }

        public async Task<Result<AuthResultDto>> RefreshAsync(string refreshToken)
        {
            var token = await _db.RefreshTokens
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Token == refreshToken);

            if (token is null || token.ExpiresAt < DateTime.UtcNow)
            {
                return new Error("users.refresh_token", Exceptions.InvalidRefreshToken, HttpStatusCode.Unauthorized);
            }

            var roles = await _userManager.GetRolesAsync(token.User);

            (string newAccessToken, var expires) = _tokenGenerator.GenerateAccessToken(token.User, roles);
            string newRefreshToken = _tokenGenerator.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken(newRefreshToken, _refreshTokenExpirationDays, token.UserId)
            {
                DeviceId = token.DeviceId
            };

            _db.RefreshTokens.Remove(token);
            _db.RefreshTokens.Add(refreshTokenEntity);

            await _db.SaveChangesAsync();

            return new AuthResultDto(newAccessToken, expires, newRefreshToken, refreshTokenEntity.ExpiresAt, token.User.UserName ?? token.User.Email!);
        }

        public async Task<Result> LogoutAsync(Guid userId, string token)
        {
            var refreshToken = _db.RefreshTokens.FirstOrDefault(t => t.Token == token && t.UserId == userId);

            if (refreshToken is null)
            {
                return new Error("users.logout", Exceptions.InvalidRefreshToken, HttpStatusCode.Unauthorized);
            }

            _db.RefreshTokens.Remove(refreshToken);
            await _db.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> InvalidateUserAccess(Guid userId)
        {
            await _db.RefreshTokens
                .Where(t => t.UserId == userId)
                .ExecuteDeleteAsync();

            return Result.Success();
        }

        internal Task<string?> GetUserName(Guid id)
            => _db.Users
            .Where(u => u.Id == id)
            .Select(u => u.UserName)
            .FirstOrDefaultAsync();
    }
}
