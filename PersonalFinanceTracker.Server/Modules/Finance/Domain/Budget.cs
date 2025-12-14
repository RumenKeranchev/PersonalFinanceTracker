namespace PersonalFinanceTracker.Server.Modules.Finance.Domain
{
    using Infrastructure;
    using PersonalFinanceTracker.Server.Infrastructure.Shared;

    public class Budget : Entity
    {
        private Budget()
        {
            Categories = [];
            Transactions = [];
        }

        public Budget(Guid userId, decimal amount, DateTime startDate, DateTime endDate) : base()
        {
            UserId = userId;
            Amount = amount;
            StartDate = startDate;
            EndDate = endDate;
            Categories = [];
            Transactions = [];
        }

        public Guid UserId { get; init; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<Category> Categories { get; init; }
        public ICollection<Transaction> Transactions { get; init; }
    }
}
