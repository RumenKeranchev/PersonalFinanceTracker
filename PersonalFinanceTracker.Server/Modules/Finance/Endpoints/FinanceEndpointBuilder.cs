namespace PersonalFinanceTracker.Server.Modules.Finance.Endpoints
{
    using Application.DTOs.Budgets;
    using Application.DTOs.Categories;
    using Application.DTOs.Transactions;
    using Application.Services;
    using Asp.Versioning.Builder;
    using Infrastructure.Requests;

    public static class FinanceEndpointBuilder
    {
        public static IEndpointRouteBuilder MapFinanceModule(this IEndpointRouteBuilder builder, ApiVersionSet versionSet)
        {
            var group = builder
                .MapGroup("/api/v{v:apiVersion}/finance")
                .WithApiVersionSet(versionSet)
                .RequireAuthorization(p => p.RequireAuthenticatedUser())
                .ProducesValidationProblem();

            group.MapPost("/transactions", async (TransactionService service, TransactionCreateDto model) =>
            {
                var result = await service.CreateAsync(model);
                return result.ToIResult();
            });

            group.MapPut("/transactions/{id}", async (Guid id, TransactionUpdateDto model, TransactionService service) =>
            {
                var result = await service.UpdateAsync(id, model);
                return result.ToIResult();
            });

            group
                .MapGet("/transactions", async (HttpRequest request, [AsParameters] PagedQuery pagedQuery, TransactionService service) =>
                {
                    var result = await service.GetAllAsync(pagedQuery, request.GetFilters(), request.GetSorters());
                    return result.ToIResult();
                })
                .Produces<TableData<TransactionListItemDto>>();

            group
                .MapGet("/transactions/{id}", async (Guid id, TransactionService service) =>
                {
                    var result = await service.GetDetailsAsync(id);
                    return result.ToIResult();
                })
                .Produces<TransactionDetailsDto>();

            group.MapPost("/categories", async (CategoryService service, CategoryCreateDto model) =>
            {
                var result = await service.CreateAsync(model);
                return result.ToIResult();
            });

            group.MapPut("/categories/{id}", async (Guid id, CategoryUpdateDto model, CategoryService service) =>
            {
                var result = await service.UpdateAsync(id, model);
                return result.ToIResult();
            });

            group
                .MapGet("/categories", async (CategoryService service) =>
                {
                    var result = await service.GetAllAsync();
                    return result.ToIResult();
                })
                .Produces<List<CategoryListItemDto>>();

            group.MapDelete("/categories/{id}", async (Guid id, CategoryService service) =>
            {
                var result = await service.DeleteAsync(id);
                return result.ToIResult();
            });

            group.MapPost("/budgets", async (BudgetService service, BudgetCreateDto model) =>
            {
                var result = await service.CreateAsync(model);
                return result.ToIResult();
            });

            group
                .MapGet("/budgets/", async (BudgetService service) =>
                {
                    var result = await service.GetAllAsync();
                    return result.ToIResult();
                })
                .Produces<List<BudgetListItemDto>>();

            group
                .MapGet("/budgets/{id}", async (Guid id, BudgetService service) =>
                {
                    var result = await service.GetDetailsAsync(id);
                    return result.ToIResult();
                })
                .Produces<List<BudgetDetailsDto>>();

            return builder;
        }
    }
}
