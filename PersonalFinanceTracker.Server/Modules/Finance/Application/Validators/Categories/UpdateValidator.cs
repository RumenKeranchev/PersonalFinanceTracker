namespace PersonalFinanceTracker.Server.Modules.Finance.Application.Validators.Categories
{
    using FluentValidation;
    using DTOs.Categories;

    public class UpdateValidator : AbstractValidator<UpdateDto>
    {
        public UpdateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.").MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
            RuleFor(x => x.Color).NotEmpty().WithMessage("Color is required.").Matches("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3}|[A-Fa-f0-9]{8})$").WithMessage("Color must be a valid hex code.");
        }
    }
}
