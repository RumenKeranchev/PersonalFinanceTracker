namespace PersonalFinanceTracker.Server.Modules.Finance.Application.Validators.Categories
{
    using DTOs.Categories;
    using FluentValidation;
    using static Resourses.Exceptions;

    public class UpdateValidator : AbstractValidator<CategoryUpdateDto>
    {
        public UpdateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(NameRequired)
                .MaximumLength(100).WithMessage(NameLength);

            RuleFor(x => x.Color)
                .NotEmpty().WithMessage(ColorRequired)
                .Matches("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3}|[A-Fa-f0-9]{8})$").WithMessage(ColorMustBeHex);
        }
    }
}
