namespace PersonalFinanceTracker.Server.Modules.Finance.Endpoints
{
    using Application;
    using Infrastructure.Requests;
    using Microsoft.AspNetCore.Mvc;
    using CreateCategoryDto = Application.DTOs.Categories.CreateDto;
    using CreateTransactionDto = Application.DTOs.Transactions.CreateDto;
    using UpdateCategoryDto = Application.DTOs.Categories.UpdateDto;
    using UpdateTransactionDto = Application.DTOs.Transactions.UpdateDto;

    public static class ReportingEnpointBuilder
    {
        public static IEndpointRouteBuilder MapFinanceModule(this IEndpointRouteBuilder builder)
        {
            builder.MapPost("/api/finance/transactions", async ([FromServices] TransactionService service, CreateTransactionDto model) =>
            {
                var errors = await service.CreateAsync(model);

                return errors is null || errors.Count == 0
                    ? Results.Ok()
                    : Results.ValidationProblem(errors.ToDictionary(k => k.PropertyName, v => new string[] { v.ErrorMessage }));
            });

            builder.MapPut("/api/finance/transactions/{id}", async (Guid id, UpdateTransactionDto model, TransactionService service) =>
            {
                var errors = await service.UpdateAsync(id, model);

                return errors is null || errors.Count == 0
                    ? Results.Ok()
                    : Results.ValidationProblem(errors.ToDictionary(k => k.PropertyName, v => new string[] { v.ErrorMessage }));
            });

            builder.MapGet("/api/finance/transactions", ([AsParameters] PagedQuery pagedQuery, TransactionService service) => service.GetAllAsync(pagedQuery));

            builder.MapGet("/api/finance/transactions/{id}", (Guid id, TransactionService service) => service.GetDetailsAsync(id));

            builder.MapPost("/api/finance/categories", ([FromServices] CategoryService service, CreateCategoryDto model) => service.CreateAsync(model));

            builder.MapPut("/api/finance/categories/{id}", (Guid id, UpdateCategoryDto model, CategoryService service) => service.UpdateAsync(id, model));

            return builder;
        }
    }
}
