namespace PersonalFinanceTracker.Server.Modules.Finance.Application.Validators.Transactions
{
    using FluentValidation;
    using PersonalFinanceTracker.Server.Modules.Finance.Application.DTOs.Transactions;

    public class CreateValidator : AbstractValidator<CreateDto>
    {
        public CreateValidator()
        {
            RuleFor(x => x.Amount).NotEqual(0).WithMessage("Amount must be different than zero.");
            RuleFor(x => x.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Date cannot be in the future.");
            RuleFor(x => x.Description).MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters.");
        }
    }
}
