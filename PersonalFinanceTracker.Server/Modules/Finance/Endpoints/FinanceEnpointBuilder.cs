namespace PersonalFinanceTracker.Server.Modules.Finance.Endpoints
{
    using Application;
    using Microsoft.AspNetCore.Mvc;
    using PersonalFinanceTracker.Server.Modules.Finance.Application.DTOs;

    public static class ReportingEnpointBuilder
    {
        public static IEndpointRouteBuilder MapFinanceModule(this IEndpointRouteBuilder builder)
        {
            builder.MapPost("/api/finance", async ([FromServices] TransactionService service, TransactionSaveDto model) =>
            {
                var errors = await service.CreateAsync(model);

                return errors is null || errors.Count == 0
                    ? Results.Ok()
                    : Results.ValidationProblem(errors.ToDictionary(k => k.PropertyName, v => new string[] { v.ErrorMessage }));
            });

            return builder;
        }
    }
}
