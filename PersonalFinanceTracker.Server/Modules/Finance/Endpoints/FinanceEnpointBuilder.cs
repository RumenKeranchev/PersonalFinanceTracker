namespace PersonalFinanceTracker.Server.Modules.Finance.Endpoints
{
    using Application;
    using Microsoft.AspNetCore.Mvc;
    using PersonalFinanceTracker.Server.Infrastructure.Requests;
    using PersonalFinanceTracker.Server.Modules.Finance.Application.DTOs.Transactions;

    public static class ReportingEnpointBuilder
    {
        public static IEndpointRouteBuilder MapFinanceModule(this IEndpointRouteBuilder builder)
        {
            builder.MapPost("/api/finance", async ([FromServices] TransactionService service, CreateDto model) =>
            {
                var errors = await service.CreateAsync(model);

                return errors is null || errors.Count == 0
                    ? Results.Ok()
                    : Results.ValidationProblem(errors.ToDictionary(k => k.PropertyName, v => new string[] { v.ErrorMessage }));
            });

            builder.MapPut("/api/finance/{id}", async (Guid id, UpdateDto model, TransactionService service) =>
            {
                var errors = await service.UpdateAsync(id, model);

                return errors is null || errors.Count == 0
                    ? Results.Ok()
                    : Results.ValidationProblem(errors.ToDictionary(k => k.PropertyName, v => new string[] { v.ErrorMessage }));
            });

            builder.MapGet("/api/finance", ([AsParameters] PagedQuery pagedQuery, TransactionService service) => service.GetAllAsync(pagedQuery));

            builder.MapGet("/api/finance/{id}", (Guid id, TransactionService service) => service.GetDetailsAsync(id));

            return builder;
        }
    }
}
