namespace PersonalFinanceTracker.Server.Modules.Finance.Application.Validators.Transactions
{
    using FluentValidation;
    using PersonalFinanceTracker.Server.Modules.Finance.Application.DTOs.Transactions;

    public class UpdateValidator : AbstractValidator<UpdateDto>
    {
        public UpdateValidator()
        {
            RuleFor(x => x.Description).MaximumLength(2000);
        }
    }
}
