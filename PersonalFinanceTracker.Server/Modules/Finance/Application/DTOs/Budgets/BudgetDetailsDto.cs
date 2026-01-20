namespace PersonalFinanceTracker.Server.Modules.Finance.Application.DTOs.Budgets
{
    public record BudgetDetailsDto(string Name, decimal Amount, DateTime StartDate, DateTime EndDate);
}
