namespace PersonalFinanceTracker.Server.Modules.Finance.Application.DTOs.Transactions
{
    public record TransactionListItemDto(decimal Amount, string Type, DateTime Date);
}
