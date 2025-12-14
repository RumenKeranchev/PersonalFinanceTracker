namespace PersonalFinanceTracker.Server.Infrastructure.Requests
{
    using FluentValidation;

    public class PagedQueryValidator : AbstractValidator<PagedQuery>
    {
        public PagedQueryValidator()
        {
            RuleFor(x => x.Index).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Size).InclusiveBetween(1, 100);
        }
    }
}
