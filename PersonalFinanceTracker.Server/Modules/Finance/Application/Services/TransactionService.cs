namespace PersonalFinanceTracker.Server.Modules.Finance.Application.Services
{
    using AutoFilter.Core;
    using Domain;
    using DTOs.Transactions;
    using Infrastructure.Requests;
    using Microsoft.EntityFrameworkCore;
    using Validators.Transactions;

    public class TransactionService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly ICurrentUser _user;

        public TransactionService(AppDbContext dbContext, ILogger<TransactionService> logger, ICurrentUser user)
        {
            _dbContext = dbContext;
            _logger = logger;
            _user = user;
        }

        public async Task<Result> CreateAsync(TransactionCreateDto model)
        {
            var validator = new CreateValidator();
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                return Result.Failure(validationResult.ToValidationError());
            }

            var transaction = new Transaction(_user.Id, model.Amount, model.Type, model.Date, model.Description);
            _dbContext.Add(transaction);

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully saved transaction [{transaction}]", transaction);

            return Result.Success();
        }

        public async Task<Result> UpdateAsync(Guid id, TransactionUpdateDto model)
        {
            var validator = new UpdateValidator();

            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                return Result.Failure(validationResult.ToValidationError());
            }

            var transaction = await _dbContext.Transactions.FindAsync(id);
            if (transaction is null || (transaction.UserId != _user.Id && !_user.IsAdmin))
            {
                return FinanceErrors.TransactionNotFound;
            }

            transaction.Description = model.Description;
            transaction.CategoryId = model.CategoryId;
            transaction.BudgetId = model.BudgetId;

            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Successfully updated transaction [{transaction}]", transaction);

            return Result.Success();
        }

        public async Task<Result<TableData<TransactionListItemDto>>> GetAllAsync(PagedQuery pagedQuery, List<Filter> filters, List<Sort> sorters)
        {
            var validationResult = new PagedQueryValidator().Validate(pagedQuery);

            if (!validationResult.IsValid)
            {
                return validationResult.ToValidationError();
            }

            var query = _dbContext.Transactions.Select(t => new TransactionListItemDto { Amount = t.Amount, Type = t.Type.ToString(), Date = t.Date });

            foreach (var filter in filters)
            {
                query = query.Apply(filter);
            }

            if (sorters.Count == 0)
            {
                sorters.Add(new(nameof(TransactionListItemDto.Date), Dir.Desc));
            }

            foreach (var sort in sorters)
            {
                query = query.Apply(sort);
            }

            var items = await query.ApplyPaging(pagedQuery).ToListAsync();
            int count = await query.CountAsync();

            return new TableData<TransactionListItemDto>(items, count % pagedQuery.Size);
        }

        public async Task<Result<TransactionDetailsDto>> GetDetailsAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return FinanceErrors.TransactionNotFound;
            }

            var model = await _dbContext.Transactions
                .Where(t => t.Id == id)
                .Select(t => new TransactionDetailsDto(t.Amount, t.Type.ToString(), t.Date, t.Description, t.Category!.Name, null))
                .FirstOrDefaultAsync();

            return model is null
                ? FinanceErrors.TransactionNotFound
                : model;
        }
    }
}
