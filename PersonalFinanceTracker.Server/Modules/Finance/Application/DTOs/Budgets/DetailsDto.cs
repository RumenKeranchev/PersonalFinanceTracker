namespace PersonalFinanceTracker.Server.Modules.Finance.Application.DTOs.Budgets
{
    public record DetailsDto(string Name, decimal Amount, DateTime StartDate, DateTime EndDate);
}
