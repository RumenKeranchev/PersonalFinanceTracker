namespace PersonalFinanceTracker.Server.Modules.Finance.Application
{
    using DTOs;
    using FluentValidation.Results;
    using PersonalFinanceTracker.Server.Infrastructure;
    using Domain;
    using Validators;
    
    public class TransactionService
    {
        private readonly AppDbContext _dbContext;

        public TransactionService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ValidationFailure>> CreateAsync(TransactionSaveDto model)
        {
            var validator = new TransactionSaveDtoValidator();
            var validationResult = validator.Validate(model);

            if (!validationResult.IsValid)
            {
                return validationResult.Errors;
            }

            var transaction = new Transaction(model.Amount, model.Type, model.Date, model.Description);
            _dbContext.Add(transaction);

            await _dbContext.SaveChangesAsync();

            return [];
        }
    }
}
