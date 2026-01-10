namespace PersonalFinanceTracker.Server.Modules.Users.Application.Validators.Auth
{
    using FluentValidation;
    using DTOs.Auth;
    using static Resourses.Exceptions;

    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(EmailRequired)
                .EmailAddress().WithMessage(EmailInvalid);

            RuleFor(x => x.Password).NotEmpty().WithMessage(PasswordRequired);
        }
    }
}
