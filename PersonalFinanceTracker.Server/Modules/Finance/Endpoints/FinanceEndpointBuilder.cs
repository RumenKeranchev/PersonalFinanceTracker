namespace PersonalFinanceTracker.Server.Modules.Finance.Endpoints
{
    using Application.Services;
    using Infrastructure.Requests;
    using CreateBudgetDto = Application.DTOs.Budgets.CreateDto;
    using CreateCategoryDto = Application.DTOs.Categories.CreateDto;
    using CreateTransactionDto = Application.DTOs.Transactions.CreateDto;
    using UpdateCategoryDto = Application.DTOs.Categories.UpdateDto;
    using UpdateTransactionDto = Application.DTOs.Transactions.UpdateDto;

    public static class FinanceEndpointBuilder
    {
        public static IEndpointRouteBuilder MapFinanceModule(this IEndpointRouteBuilder builder)
        {
            var group = builder.MapGroup("/api/finance");

            group.MapPost("/transactions", async (TransactionService service, CreateTransactionDto model) =>
            {
                var result = await service.CreateAsync(model);
                return result.ToIResult();
            });

            group.MapPut("/transactions/{id}", async (Guid id, UpdateTransactionDto model, TransactionService service) =>
            {
                var result = await service.UpdateAsync(id, model);
                return result.ToIResult();
            });

            group.MapGet("/transactions", async ([AsParameters] PagedQuery pagedQuery, TransactionService service) =>
            {
                var result = await service.GetAllAsync(pagedQuery);
                return result.ToIResult();
            });

            group.MapGet("/transactions/{id}", async (Guid id, TransactionService service) =>
            {
                var result = await service.GetDetailsAsync(id);
                return result.ToIResult();
            });

            group.MapPost("/categories", async (CategoryService service, CreateCategoryDto model) =>
            {
                var result = await service.CreateAsync(model);
                return result.ToIResult();
            });

            group.MapPut("/categories/{id}", async (Guid id, UpdateCategoryDto model, CategoryService service) =>
            {
                var result = await service.UpdateAsync(id, model);
                return result.ToIResult();
            });

            group.MapGet("/categories", async (CategoryService service) =>
            {
                var result = await service.GetAllAsync();
                return result.ToIResult();
            });

            group.MapDelete("/categories/{id}", async (Guid id, CategoryService service) =>
            {
                var result = await service.DeleteAsync(id);
                return result.ToIResult();
            });

            group.MapPost("/budgets", async (BudgetService service, CreateBudgetDto model) =>
            {
                var result = await service.CreateAsync(model);
                return result.ToIResult();
            });

            group.MapGet("/budgets/", async (BudgetService service) =>
            {
                var result = await service.GetAllAsync();
                return result.ToIResult();
            });

            group.MapGet("/budgets/{id}", async (Guid id, BudgetService service) =>
            {
                var result = await service.GetDetailsAsync(id);
                return result.ToIResult();
            });

            return builder;
        }
    }
}
