namespace PersonalFinanceTracker.Server.Modules.Finance.Domain
{
    using PersonalFinanceTracker.Server.Infrastructure.Shared;

    public class Budget : Entity
    {
        private Budget()
        {
            Categories = [];
            Transactions = [];
            Name = string.Empty;
        }

        public Budget(Guid userId, decimal amount, string name, DateTime startDate, DateTime endDate) : base()
        {
            UserId = userId;
            Amount = amount;
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            Categories = [];
            Transactions = [];
        }

        public Guid UserId { get; init; }
        public decimal Amount { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<Category> Categories { get; init; }
        public ICollection<Transaction> Transactions { get; init; }
    }
}
