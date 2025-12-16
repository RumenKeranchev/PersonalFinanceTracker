namespace PersonalFinanceTracker.Server.Modules.Finance.Application
{
    using Domain;
    using DTOs.Transactions;
    using FluentValidation.Results;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using PersonalFinanceTracker.Server.Infrastructure.Extensions;
    using PersonalFinanceTracker.Server.Infrastructure.Requests;
    using Validators.Transactions;

    public class TransactionService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;

        public TransactionService(AppDbContext dbContext, ILogger<TransactionService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<ValidationFailure>> CreateAsync(CreateDto model)
        {
            var validator = new CreateValidator();
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                return validationResult.Errors;
            }

            // TODO: Replace Guid.Empty with actual UserId when authentication is implemented
            var transaction = new Transaction(Guid.Empty, model.Amount, model.Type, model.Date, model.Description);
            _dbContext.Add(transaction);

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successuly saved transaction [{transaction}]", transaction);

            return [];
        }

        public async Task<List<ValidationFailure>> UpdateAsync(Guid id, UpdateDto model)
        {
            var validator = new UpdateValidator();

            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                return validationResult.Errors;
            }

            var transaction = await _dbContext.Transactions.FindAsync(id);
            if (transaction is null)
            {
                return [new ValidationFailure("Id", "Transaction not found")];
            }

            transaction.Description = model.Description;
            transaction.CategoryId = model.CategoryId;
            transaction.BudgetId = model.BudgetId;

            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Successfully updated transaction [{transaction}]", transaction);
            return [];
        }

        public Task<List<ListItemDto>> GetAllAsync(PagedQuery pagedQuery)
        {
            var validator = new PagedQueryValidator();
            var validationResult = validator.Validate(pagedQuery);

            return !validationResult.IsValid
                ? throw new ArgumentException("Invalid paging parameters", nameof(pagedQuery))
                : _dbContext.Transactions
                    .OrderBy(t => t.Id)
                    .Select(t => new ListItemDto(t.Amount, t.Type.ToString(), t.Date))
                    .ApplyPaging(pagedQuery)
                    .ToListAsync();
        }

        public async Task<DetailsDto?> GetDetailsAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Id cannot be empty", nameof(id));
            }

            var model = await _dbContext.Transactions
                .Where(t => t.Id == id)
                .Select(t => new DetailsDto(t.Amount, t.Type.ToString(), t.Date, t.Description, t.Category.Name, null))
                .FirstOrDefaultAsync();

            return model is null
                ? throw new KeyNotFoundException("Transaction not found")
                : model;
        }
    }
}
