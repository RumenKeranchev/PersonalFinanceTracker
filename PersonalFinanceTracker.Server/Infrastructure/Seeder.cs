namespace PersonalFinanceTracker.Server.Infrastructure
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Modules.Finance.Domain;
    using Modules.Users.Application;
    using Modules.Users.Domain;

    public static class Seeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            await db.Database.MigrateAsync();

            int usersCount = await userManager.Users.CountAsync();

            if (usersCount > 0)
            {
                return;
            }

            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(userManager, "test@test.com", "Test123_", Roles.Admin);
            await SeedUsersAsync(userManager, "user@whatever.com", "User1!", Roles.User);
            await SeedFinanceAsync(db, userManager);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
        {
            string[] roles = [Roles.Admin, Roles.User];

            foreach (string? role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }
        }

        private static async Task SeedUsersAsync(UserManager<AppUser> userManager, string email, string password, string role)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return;
            }

            user = new AppUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            await userManager.AddToRoleAsync(user, role);
        }

        private static async Task SeedFinanceAsync(AppDbContext db, UserManager<AppUser> userManager)
        {
            var user = await userManager.FindByEmailAsync("user@whatever.com");

            db.Budgets.Add(new Budget(user!.Id, 2000, "Monthly Budget", DateTime.UtcNow, DateTime.UtcNow.AddMonths(1)));

            db.Categories.Add(new Category("Food", "#bf9d02"));
            db.Categories.Add(new Category("Food", "#4707b5"));

            db.Transactions.Add(new Transaction(user!.Id, 50, TransactionType.Expense));

            await db.SaveChangesAsync();
        }
    }
}
