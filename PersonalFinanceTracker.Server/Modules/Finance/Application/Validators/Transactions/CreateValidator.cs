namespace PersonalFinanceTracker.Server.Modules.Finance.Application.Validators.Transactions
{
    using FluentValidation;
    using PersonalFinanceTracker.Server.Modules.Finance.Application.DTOs.Transactions;
    using static Resourses.Exceptions;

    public class CreateValidator : AbstractValidator<CreateDto>
    {
        public CreateValidator()
        {
            RuleFor(x => x.Amount).NotEqual(0).WithMessage(TransactionAmount);

            RuleFor(x => x.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(TransactionDate);

            RuleFor(x => x.Description).MaximumLength(2000).WithMessage(DescriptionLength);
        }
    }
}
