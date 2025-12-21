namespace PersonalFinanceTracker.Server.Modules.Finance.Endpoints
{
    using Infrastructure.Requests;
    using Microsoft.AspNetCore.Mvc;
    using Application.Services;
    using CreateCategoryDto = Application.DTOs.Categories.CreateDto;
    using CreateTransactionDto = Application.DTOs.Transactions.CreateDto;
    using UpdateCategoryDto = Application.DTOs.Categories.UpdateDto;
    using UpdateTransactionDto = Application.DTOs.Transactions.UpdateDto;

    public static class ReportingEndpointBuilder
    {
        public static IEndpointRouteBuilder MapFinanceModule(this IEndpointRouteBuilder builder)
        {
            builder.MapPost("/api/finance/transactions", async ([FromServices] TransactionService service, CreateTransactionDto model) =>
            {
                var result = await service.CreateAsync(model);
                return result.ToIResult();
            });

            builder.MapPut("/api/finance/transactions/{id}", async (Guid id, UpdateTransactionDto model, TransactionService service) =>
            {
                var result = await service.UpdateAsync(id, model);
                return result.ToIResult();
            });

            builder.MapGet("/api/finance/transactions", ([AsParameters] PagedQuery pagedQuery, TransactionService service) => service.GetAllAsync(pagedQuery));

            builder.MapGet("/api/finance/transactions/{id}", (Guid id, TransactionService service) => service.GetDetailsAsync(id));

            builder.MapPost("/api/finance/categories", async ([FromServices] CategoryService service, CreateCategoryDto model) =>
            {
                var result = await service.CreateAsync(model);
                return result.ToIResult();
            });

            builder.MapPut("/api/finance/categories/{id}", async (Guid id, UpdateCategoryDto model, CategoryService service) =>
            {
                var result = await service.UpdateAsync(id, model);
                return result.ToIResult();
            });

            builder.MapGet("/api/finance/categories", (CategoryService service) => service.GetAllAsync());

            builder.MapDelete("/api/finance/categories/{id}", async (Guid id, CategoryService service) =>
            {
                var result = await service.DeleteAsync(id);
                return result.ToIResult();
            });

            return builder;
        }
    }
}
