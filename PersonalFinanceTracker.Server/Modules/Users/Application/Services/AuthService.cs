namespace PersonalFinanceTracker.Server.Modules.Users.Application.Services
{
    using Application.DTOs.Auth;
    using Domain;
    using Infrastructure.Requests;
    using Microsoft.AspNetCore.Identity;
    using DTOs.Validators.Auth;
    using PersonalFinanceTracker.Server.Modules.Users.Application.DTOs.Validators;

    public class AuthService
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger _logger;

        public AuthService(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ILogger<AuthService> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<Result<string>> RegisterAsync(RegisterDto model)
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
                return new Error("users.auth.creation_failed", string.Join(", ", identityRes.Errors?.Select(e => e.Description) ?? []));
            }

            identityRes = await _userManager.AddToRoleAsync(user, Roles.User);

            if (!identityRes.Succeeded)
            {
                return new Error("users.auth.role_assignment_failed", string.Join(", ", identityRes.Errors?.Select(e => e.Description) ?? []));
            }

            var signInRes = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);
            if (!signInRes.Succeeded)
            {
                return UsersErrors.InvalidCredentials;
            }

            //TODO: generate token
            string token = "";

            _logger.LogInformation("User [{Email}] registered successfully.", model.Email);

            return token;
        }

        public async Task<Result<string>> LoginAsync(LoginDto model)
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

            // TODO: generate token
            string token = "";

            return token;
        }
    }
}
