namespace PersonalFinanceTracker.Server.Modules.Finance.Application.Services
{
    using Domain;
    using DTOs.Budgets;
    using Infrastructure.Requests;
    using Microsoft.EntityFrameworkCore;
    using Validators.Budgets;

    public class BudgetService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly ICurrentUser _user;

        public BudgetService(AppDbContext dbContext, ILogger<BudgetService> logger, ICurrentUser user)
        {
            _dbContext = dbContext;
            _logger = logger;
            _user = user;
        }

        public async Task<Result> CreateAsync(BudgetCreateDto model)
        {
            var validator = new CreateValidator();
            var validationResult = await validator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                return Result.Failure(validationResult.ToValidationError());
            }

            var budget = new Budget(_user.Id, model.Amount, model.Name, model.StartDate, model.EndDate);

            await _dbContext.Budgets.AddAsync(budget);

            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Budget created with Id: {BudgetId}", budget.Id);

            return Result.Success();
        }

        public async Task<Result<List<BudgetListItemDto>>> GetAllAsync()
        {
            var budgets = await _dbContext.Budgets
                 .Select(b => new BudgetListItemDto(
                     b.Id,
                     b.Name,
                     b.Amount,
                     b.EndDate >= DateTime.UtcNow,
                     (b.EndDate - DateTime.UtcNow).Days,
                     b.Categories.Select(c => new DTOs.Categories.CategoryListItemForBudgetDto(c.Name, c.Color)).ToList()
                 ))
                 .ToListAsync();

            return budgets;
        }

        public async Task<Result<BudgetDetailsDto>> GetDetailsAsync(Guid id)
        {
            var budget = await _dbContext.Budgets
                .Where(b => b.Id == id)
                .Select(b => new BudgetDetailsDto(b.Name, b.Amount, b.StartDate, b.EndDate))
                .FirstOrDefaultAsync();

            return budget is null
                ? Error.InvalidId
                : budget;
        }
    }
}
