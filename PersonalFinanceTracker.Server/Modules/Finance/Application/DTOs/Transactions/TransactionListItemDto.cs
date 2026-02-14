namespace PersonalFinanceTracker.Server.Modules.Finance.Application.DTOs.Transactions
{
    public record TransactionListItemDto
    {
        public decimal Amount { get; init; }

        public string Type { get; init; } = default!;

        public DateTime Date { get; init; }
    }
}
