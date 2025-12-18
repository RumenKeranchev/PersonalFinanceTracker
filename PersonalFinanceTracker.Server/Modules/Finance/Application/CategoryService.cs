namespace PersonalFinanceTracker.Server.Modules.Finance.Application
{
    using Domain;
    using DTOs.Categories;
    using FluentValidation;
    using Infrastructure;
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

        public async Task CreateAsync(CreateDto model)
        {
            var validator = new CreateValidator();
            var result = validator.Validate(model);

            if (!result.IsValid)
            {
                return;
            }

            var category = new Category(model.Name);

            _dbContext.Categories.Add(category);

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Category [{CategoryName}] created with Id [{CategoryId}]", category.Name, category.Id);
        }

        public async Task UpdateAsync(Guid id, UpdateDto model)
        {
            var validator = new UpdateValidator();
            var result = validator.Validate(model);

            if (!result.IsValid)
            {
                return;
            }

            var category = await _dbContext.Categories.FindAsync(id);

            if (category is null)
            {
                _logger.LogWarning("Category with Id [{CategoryId}] not found", id);
                return;
            }

            category.Name = model.Name;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Category [{CategoryName}] created with Id [{CategoryId}]", category.Name, category.Id);
        }
    }
}
