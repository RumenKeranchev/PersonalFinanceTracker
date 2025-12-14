namespace PersonalFinanceTracker.Server.Modules.Finance.Application.DTOs.Transactions
{
    public record UpdateDto(string? Description, Guid? CategoryId, Guid? BudgetId);
}
