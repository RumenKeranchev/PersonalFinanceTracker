namespace PersonalFinanceTracker.Server.Modules.Finance.Application.Validators.Budgets
{
    using DTOs.Budgets;
    using FluentValidation;

    public class CreateValidator : AbstractValidator<CreateDto>
    {
        public CreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Budget name is required.")
                .MaximumLength(100).WithMessage("Budget name must not exceed 100 characters.");

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Budget amount must be greater than zero.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required.")
                .LessThan(x => x.EndDate).WithMessage("Start date must be earlier than end date.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required.")
                .GreaterThan(x => x.StartDate).WithMessage("End date must be later than start date.");
        }
    }
}
