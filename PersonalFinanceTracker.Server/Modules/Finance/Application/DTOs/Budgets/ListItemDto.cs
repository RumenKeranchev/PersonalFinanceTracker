namespace PersonalFinanceTracker.Server.Modules.Finance.Application.DTOs.Budgets
{
    using Categories;

    public record ListItemDto(Guid Id, string Name, decimal Amount, bool IsValid, int ExpiresIn, List<ListItemForBudgetDto> Categories);
}
