namespace PersonalFinanceTracker.Server.Modules.Finance.Application.DTOs.Budgets
{
    using Categories;

    public record BudgetListItemDto(Guid Id, string Name, decimal Amount, bool IsValid, int ExpiresIn, List<CategoryListItemForBudgetDto> Categories);
}
