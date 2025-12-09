namespace PersonalFinanceTracker.Server.Modules.Finance.Application.Validators
{
    using FluentValidation;
    using DTOs;

    public class TransactionSaveDtoValidator : AbstractValidator<TransactionSaveDto>
    {
        public TransactionSaveDtoValidator()
        {
            RuleFor(x => x.Amount).NotEqual(0).WithMessage("Amount must be different than zero.");
            RuleFor(x => x.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Date cannot be in the future.");
            RuleFor(x => x.Description).MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters.");            
        }
    }
}
