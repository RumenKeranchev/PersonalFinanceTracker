namespace PersonalFinanceTracker.Server.Modules.Finance.Persistence
{
    using Finance.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Amount).IsRequired().HasPrecision(18, 3);

            builder.Property(t => t.Type).IsRequired();

            builder.Property(t => t.Description).HasMaxLength(2000);

            builder
                .HasOne(t => t.Category)
                .WithMany(c => c.Transactions)
                .IsRequired(false)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasOne(t => t.Budget)
                .WithMany(c => c.Transactions)
                .IsRequired(false)
                .HasForeignKey(t => t.BudgetId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
