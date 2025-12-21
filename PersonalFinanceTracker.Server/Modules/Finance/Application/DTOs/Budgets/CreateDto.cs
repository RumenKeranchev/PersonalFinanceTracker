namespace PersonalFinanceTracker.Server.Modules.Finance.Application.DTOs.Budgets
{
    public record CreateDto(string Name, decimal Amount, DateTime StartDate, DateTime EndDate);
}
