namespace PersonalFinanceTracker.Server.Modules.Users.Application.Services
{
    using Domain;
    using DTOs.Auth;
    using DTOs.Validators;
    using DTOs.Validators.Auth;
    using Infrastructure.Requests;
    using Microsoft.AspNetCore.Identity;

    public class AuthService
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger _logger;
        private readonly TokenGenerator _tokenGenerator;

        public AuthService(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ILogger<AuthService> logger, TokenGenerator tokenGenerator)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _tokenGenerator = tokenGenerator;
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

            var token = _tokenGenerator.GenerateToken(user);

            _logger.LogInformation("User [{Email}] registered successfully.", model.Email);

            return token;
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

            var token = _tokenGenerator.GenerateToken(user);

            return token;
        }

        public Task<Result<AuthResultDto>> RefreshAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result> LogoutAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
