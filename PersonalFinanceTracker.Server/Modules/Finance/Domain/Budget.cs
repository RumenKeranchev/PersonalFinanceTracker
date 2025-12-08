namespace PersonalFinanceTracker.Server.Modules.Finance.Domain
{
    public class Budget
    {
        private Budget()
        {
            Categories = [];
            Transactions = [];
        }

        public Budget(Guid userId, decimal amount, DateTime startDate, DateTime endDate)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Amount = amount;
            StartDate = startDate;
            EndDate = endDate;
            Categories = [];
            Transactions = [];
        }

        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<Category> Categories { get; init; }
        public ICollection<Transaction> Transactions { get; init; }
    }
}
