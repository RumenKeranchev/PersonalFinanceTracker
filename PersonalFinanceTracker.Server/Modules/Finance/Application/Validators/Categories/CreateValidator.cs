namespace PersonalFinanceTracker.Server.Modules.Finance.Application.Validators.Categories
{
    using FluentValidation;
    using DTOs.Categories;

    public class CreateValidator : AbstractValidator<CreateDto>
    {
        public CreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.").MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
        }
    }
}
