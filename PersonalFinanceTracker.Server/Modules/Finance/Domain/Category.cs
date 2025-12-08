namespace PersonalFinanceTracker.Server.Modules.Finance.Domain
{
    using Infrastructure;

    public class Category : Entity
    {
        private Category()
        {
            Name = string.Empty;
            Transactions = [];
            Budgets = [];
        }

        public Category(string name) : base()
        {
            Name = name;
            Transactions = [];
            Budgets = [];
        }

        public string Name { get; set; }

        public ICollection<Transaction> Transactions { get; init; }
        public ICollection<Budget> Budgets { get; init; }
    }
}
