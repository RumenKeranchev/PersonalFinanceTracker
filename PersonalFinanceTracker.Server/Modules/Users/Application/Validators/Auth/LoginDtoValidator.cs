namespace PersonalFinanceTracker.Server.Modules.Users.Application.Validators.Auth
{
    using DTOs.Auth;
    using FluentValidation;
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
