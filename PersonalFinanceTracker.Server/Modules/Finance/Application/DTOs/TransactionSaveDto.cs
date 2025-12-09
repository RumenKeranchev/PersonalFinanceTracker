namespace PersonalFinanceTracker.Server.Modules.Finance.Application.DTOs
{
    using Domain;

    public record TransactionSaveDto(decimal Amount, TransactionType Type, DateTime? Date, string? Description);
}
