namespace PersonalFinanceTracker.Server.Modules.Users.Application.DTOs.Validators
{
    using FluentValidation;
    using PersonalFinanceTracker.Server.Modules.Users.Application.DTOs.Auth;

    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
