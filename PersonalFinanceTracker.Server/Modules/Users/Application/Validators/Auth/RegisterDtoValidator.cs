namespace PersonalFinanceTracker.Server.Modules.Users.Application.Validators.Auth
{
    using FluentValidation;
    using DTOs.Auth;
    using static Resourses.Exceptions;

    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage(EmailRequired)
                .EmailAddress().WithMessage(EmailInvalid);

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage(UsernameRequired)
                .MinimumLength(3).WithMessage(UsernameLength);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(PasswordRequired)
                .MinimumLength(6).WithMessage(PasswordLength)
                .Matches("\\d+").WithMessage(PasswordDigit)
                .Matches("[^a-zA-Z0-9]+").WithMessage(PasswordSpecialCharacter)
                .Matches("[A-Z]").WithMessage(PasswordUpperCase);
        }
    }
}
