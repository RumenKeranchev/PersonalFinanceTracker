namespace PersonalFinanceTracker.Server.Modules.Finance.Application.DTOs.Transactions
{
    using Domain;

    public record CreateDto(decimal Amount, TransactionType Type, DateTime? Date, string? Description);
}
