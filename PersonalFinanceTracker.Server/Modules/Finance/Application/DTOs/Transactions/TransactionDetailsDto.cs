namespace PersonalFinanceTracker.Server.Modules.Finance.Application.DTOs.Transactions
{
    public record TransactionDetailsDto(decimal Amount, string Type, DateTime Date, string? Description, string? Category, string? Budget);
}
