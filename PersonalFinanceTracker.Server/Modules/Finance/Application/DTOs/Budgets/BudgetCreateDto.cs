namespace PersonalFinanceTracker.Server.Modules.Finance.Application.DTOs.Budgets
{
    public record BudgetCreateDto(string Name, decimal Amount, DateTime StartDate, DateTime EndDate);
}
