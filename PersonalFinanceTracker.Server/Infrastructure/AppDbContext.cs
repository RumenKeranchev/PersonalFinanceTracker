namespace PersonalFinanceTracker.Server.Infrastructure
{
    using Microsoft.EntityFrameworkCore;
    using PersonalFinanceTracker.Server.Modules.Finance.Domain;

    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
