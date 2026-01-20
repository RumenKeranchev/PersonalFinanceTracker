namespace PersonalFinanceTracker.Server.Modules.Finance.Application.Validators.Budgets
{
    using DTOs.Budgets;
    using FluentValidation;
    using static Resourses.Exceptions;

    public class CreateValidator : AbstractValidator<BudgetCreateDto>
    {
        public CreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(NameRequired)
                .MaximumLength(100).WithMessage(NameLength);

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage(BudgetAmount);

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage(BudgetStartDate)
                .LessThan(x => x.EndDate).WithMessage(BudgetStartDateAfterEndDate);

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage(BudgetEndDate)
                .GreaterThan(x => x.StartDate).WithMessage(BudgetEndDateBeforeStartDate);
        }
    }
}
