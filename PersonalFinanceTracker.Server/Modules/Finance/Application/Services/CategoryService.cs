namespace PersonalFinanceTracker.Server.Modules.Finance.Application.Services
{
    using Domain;
    using DTOs.Categories;
    using Infrastructure.Requests;
    using System;
    using Validators.Categories;

    internal class CategoryService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;

        public CategoryService(AppDbContext dbContext, ILogger<CategoryService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Result> CreateAsync(CreateDto model)
        {
            var validator = new CreateValidator();
            var result = validator.Validate(model);

            if (!result.IsValid)
            {
                return Result.Failure(result.ToValidationError());
            }

            var category = new Category(model.Name, model.Color);

            _dbContext.Categories.Add(category);

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Category [{CategoryName}] created with Id [{CategoryId}]", category.Name, category.Id);

            return Result.Success();
        }

        public async Task<Result> UpdateAsync(Guid id, UpdateDto model)
        {
            var validator = new UpdateValidator();
            var result = validator.Validate(model);

            if (!result.IsValid)
            {
                return Result.Failure(result.ToValidationError());
            }

            var category = await _dbContext.Categories.FindAsync(id);

            if (category is null)
            {
                _logger.LogWarning("Category with Id [{CategoryId}] not found", id);
                return Error.InvalidId;
            }

            category.Name = model.Name;
            category.Color = model.Color;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Category [{CategoryName}] created with Id [{CategoryId}]", category.Name, category.Id);

            return Result.Success();
        }
    }
}
