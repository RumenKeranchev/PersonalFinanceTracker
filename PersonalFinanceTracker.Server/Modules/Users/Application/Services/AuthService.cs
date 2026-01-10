namespace PersonalFinanceTracker.Server.Modules.Users.Application.Services
{
    using Domain;
    using DTOs.Auth;
    using Infrastructure.Requests;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Resourses;
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

            string token = _tokenGenerator.GenerateAccessToken(user);
            string refreshToken = _tokenGenerator.GenerateRefreshToken();

            _logger.LogInformation("User [{Email}] registered successfully.", model.Email);

            return new AuthResultDto(token, refreshToken);
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

            string token = _tokenGenerator.GenerateAccessToken(user);
            string refreshToken = _tokenGenerator.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(_refreshTokenExpirationDays)
            };

            _db.RefreshTokens.Add(refreshTokenEntity);

            await _db.SaveChangesAsync();

            _logger.LogInformation("User [{Email}] logged in successfully.", model.Email);

            return new AuthResultDto(token, refreshToken);
        }

        public async Task<Result<AuthResultDto>> RefreshAsync(string refreshToken)
        {
            var token = await _db.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken);

            if (token is null || token.ExpiresAt < DateTime.UtcNow)
            {
                throw new ApplicationException(Exceptions.InvalidRefreshToken);
            }

            var newerToken = await _db.RefreshTokens
                .Where(t => t.UserId == token.UserId && t.ExpiresAt > token.ExpiresAt)
                .Select(t => t.Id)
                .FirstOrDefaultAsync();

            if (newerToken != Guid.Empty)
            {
                throw new ApplicationException(Exceptions.InvalidRefreshToken);
            }

            await _db.Entry(token).Reference(t => t.User).LoadAsync();

            string newAccessToken = _tokenGenerator.GenerateAccessToken(token.User);
            string newRefreshToken = _tokenGenerator.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Token = newRefreshToken,
                UserId = token.UserId,
                ExpiresAt = DateTime.UtcNow.AddDays(_refreshTokenExpirationDays)
            };

            _db.RefreshTokens.Add(refreshTokenEntity);

            await _db.SaveChangesAsync();

            return new AuthResultDto(newAccessToken, newRefreshToken);
        }

        public Task<Result> LogoutAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
