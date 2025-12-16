namespace PersonalFinanceTracker.Server.Modules.Finance.Persistence
{
    using Finance.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
    {
        public void Configure(EntityTypeBuilder<Budget> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Amount)
                   .IsRequired()
                   .HasPrecision(18, 2);

            builder.Property(b => b.StartDate)
                   .IsRequired();

            builder.Property(b => b.EndDate)
                   .IsRequired();

            builder.HasMany(b => b.Transactions)
                   .WithOne(t => t.Budget)
                   .HasForeignKey(t => t.BudgetId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
