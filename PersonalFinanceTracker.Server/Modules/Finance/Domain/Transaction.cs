namespace PersonalFinanceTracker.Server.Modules.Finance.Domain
{
    using Infrastructure;

    public class Transaction : Entity
    {
        private Transaction() { }
        
        public Transaction(decimal amount, TransactionType type, DateTime? date = null, string? description = null, Guid? categoryId = null, Guid? budgetId = null)
            : base()
        {
            Amount = amount;
            Type = type;
            Date = date ?? DateTime.UtcNow;
            Description = description;
            CategoryId = categoryId;
            BudgetId = budgetId;
        }

        public decimal Amount { get; init; }
        public TransactionType Type { get; init; }
        public DateTime Date { get; init; }
        public string? Description { get; set; }

        public Guid? CategoryId { get; set; }
        public Category? Category { get; set; }

        public Guid? BudgetId { get; set; }
        public Budget? Budget { get; set; }

        public Guid CreatorId { get; set; }
    }
}
