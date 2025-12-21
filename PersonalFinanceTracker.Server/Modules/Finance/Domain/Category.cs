namespace PersonalFinanceTracker.Server.Modules.Finance.Domain
{
    using PersonalFinanceTracker.Server.Infrastructure.Shared;

    public class Category : Entity
    {
        private Category()
        {
            Name = string.Empty;
            Color = string.Empty;
            Transactions = [];
            Budgets = [];
        }

        public Category(string name, string color) : base()
        {
            Name = name;
            Transactions = [];
            Budgets = [];
            Color = color;
        }

        public string Name { get; set; }
        public string Color { get; set; }

        public ICollection<Transaction> Transactions { get; init; }
        public ICollection<Budget> Budgets { get; init; }
    }
}
