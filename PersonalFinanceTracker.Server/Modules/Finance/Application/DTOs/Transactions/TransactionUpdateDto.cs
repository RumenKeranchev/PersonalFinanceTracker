namespace PersonalFinanceTracker.Server.Modules.Finance.Application.DTOs.Transactions
{
    public record TransactionUpdateDto(string? Description, Guid? CategoryId, Guid? BudgetId);
}
