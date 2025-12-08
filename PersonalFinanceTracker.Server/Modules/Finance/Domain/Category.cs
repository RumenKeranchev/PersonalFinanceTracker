namespace PersonalFinanceTracker.Server.Modules.Finance.Domain
{
    public class Category
    {
        private Category()
        {
            Name = string.Empty;
            Transactions = [];
            Budgets = [];
        }

        public Category(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
            Transactions = [];
            Budgets = [];
        }

        public Guid Id { get; init; }
        public string Name { get; set; }

        public ICollection<Transaction> Transactions { get; init; }
        public ICollection<Budget> Budgets { get; init; }
    }
}
