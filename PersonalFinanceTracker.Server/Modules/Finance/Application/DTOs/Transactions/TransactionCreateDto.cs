namespace PersonalFinanceTracker.Server.Modules.Finance.Application.DTOs.Transactions
{
    using Domain;

    public record TransactionCreateDto(decimal Amount, TransactionType Type, DateTime? Date, string? Description);
}
