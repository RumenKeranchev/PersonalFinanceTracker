namespace PersonalFinanceTracker.Server.Infrastructure
{
    using Bogus;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Modules.Finance.Domain;
    using Modules.Users.Application;
    using Modules.Users.Domain;

    public static class Seeder
    {
        public static List<Category> Categories = [];
        public static List<Transaction> Transactions = [];
        public static List<Budget> Budgets = [];
        private static List<AppUser> Users = [];
        private const string AdminEmail = "test@test.com";

        public static async Task<bool> SeedUsersAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            await db.Database.MigrateAsync();

            int usersCount = await userManager.Users.CountAsync();

            if (usersCount > 0)
            {
                return false;
            }

            string[] roles = [Roles.Admin, Roles.User];

            foreach (string? role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }

            var admin = new AppUser(AdminEmail, "admin");
            var user = new AppUser("user@whatever.com", "user");

            var result = await userManager.CreateAsync(admin, "Test123_");
            await userManager.AddToRoleAsync(admin, Roles.Admin);

            result = await userManager.CreateAsync(user, "User1!");
            await userManager.AddToRoleAsync(user, Roles.User);

            var usersFaker = new Faker<AppUser>()
                .RuleFor(u => u.UserName, (f, u) => f.Internet.UserName())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.UserName))
                .RuleFor(u => u.EmailConfirmed, f => true);

            Users = usersFaker.Generate(10);

            foreach (var fake in Users)
            {
                result = await userManager.CreateAsync(fake, "P@ssw0rd!");
                await userManager.AddToRoleAsync(fake, Roles.User);
            }

            return true;
        }

        public static void FakeData()
        {
            List<string> categoryNames = ["Food", "Transport", "Entertainment", "Utilities", "Health", "Education", "Shopping", "Travel", "Savings", "Miscellaneous"];
            List<string> budgetNames = ["Monthly Budget", "Vacation Fund", "Emergency Fund", "Gadget Savings", "Holiday Shopping", "Car Maintenance", "Home Improvement", "Education Fund", "Fitness Budget", "Dining Out"];
            int budgetIndex = 0;
            var userIds = Users.Where(u => !u.Email!.Equals(AdminEmail)).Select(u => u.Id).ToList();
            var rand = new Random();

            foreach (string name in categoryNames)
            {
                Categories.Add(new Category(name, $"#{rand.Next(0x1000000):X6}"));
            }

            var transactionFaker = new Faker<Transaction>()
                .CustomInstantiator(t => new(
                    t.PickRandom(userIds),
                    t.Finance.Amount(-7897, 250_000),
                    t.PickRandom<TransactionType>(),
                    t.Date.Between(DateTime.UtcNow.AddYears(-1), DateTime.UtcNow.AddDays(-1)),
                    t.Lorem.Sentence(),
                    t.PickRandom(Categories).Id
                    ));

            var budgetFaker = new Faker<Budget>()
                .CustomInstantiator(b => new(
                    b.PickRandom(userIds),
                    b.Finance.Amount(500, 80_000),
                    budgetNames[budgetIndex++],
                    b.Date.Between(DateTime.UtcNow.AddMonths(-10), DateTime.UtcNow.AddDays(-3)),
                    b.Date.Between(DateTime.UtcNow.AddMonths(-2), DateTime.UtcNow.AddDays(-2))
                    ))
                .RuleFor(b => b.Categories, b => [b.PickRandom(Categories)])
                .RuleFor(b => b.Transactions, (f, b) =>
                {
                    var transactions = transactionFaker.GenerateBetween(1, 10);

                    transactions.ForEach(b.Transactions.Add);
                    Transactions.AddRange(transactions);

                    return b.Transactions;
                });

            var transactions = transactionFaker.Generate(1000);
            Transactions.AddRange(transactions);

            var budgets = budgetFaker.Generate(budgetNames.Count);
            Budgets.AddRange(budgets);
        }
    }
}
