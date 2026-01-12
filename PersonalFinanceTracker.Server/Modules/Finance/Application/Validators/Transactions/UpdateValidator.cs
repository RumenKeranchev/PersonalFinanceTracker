namespace PersonalFinanceTracker.Server.Modules.Finance.Application.Validators.Transactions
{
    using DTOs.Transactions;
    using FluentValidation;
    using static Resourses.Exceptions;

    public class UpdateValidator : AbstractValidator<UpdateDto>
    {
        public UpdateValidator()
        {
            RuleFor(x => x.Description).MaximumLength(2000).WithMessage(DescriptionLength);
        }
    }
}
