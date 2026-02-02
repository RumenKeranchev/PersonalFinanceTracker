namespace PersonalFinanceTracker.Server.Modules.Reporting.Application
{
    using DTOs;
    using Finance.Domain;
    using Infrastructure.Requests;
    using Microsoft.EntityFrameworkCore;
    using System.Globalization;

    public class ReportingService
    {
        private readonly AppDbContext _dbContext;
        private readonly ICurrentUser _user;

        public ReportingService(AppDbContext dbContext, ICurrentUser user)
        {
            _dbContext = dbContext;
            _user = user;
        }

        public async Task<Result<Dashboard>> GetDashboardAsync()
        {
            var transactions = await _dbContext.Transactions
                .Where(t => t.UserId == _user.Id)
                .Select(t => new
                {
                    t.Date.Date,
                    t.Type,
                    t.Amount
                })
                .ToListAsync();

            var labels = transactions
                .OrderBy(t => t.Date)
                .Select(t => CultureInfo.InvariantCulture.TextInfo.ToTitleCase(t.Date.ToString("MMMM yyyy")))
                .Distinct()
                .ToList();

            var incomes = new DashboardDataset("Income",
                labels
                .Select(l =>
                    transactions
                        .Where(t => t.Type == TransactionType.Income && t.Date.ToString("MMM yyyy").Equals(l, StringComparison.InvariantCultureIgnoreCase))
                        .Select(t => t.Amount)
                        .DefaultIfEmpty()
                        .Sum())
                .ToList());

            var expenses = new DashboardDataset("Expenses",
                 labels
                 .Select(l =>
                     transactions
                         .Where(t => t.Type == TransactionType.Expense && t.Date.ToString("MMM yyyy").Equals(l, StringComparison.InvariantCultureIgnoreCase))
                         .Select(t => t.Amount)
                         .DefaultIfEmpty()
                         .Sum())
                 .ToList());

            return new Dashboard(labels, [incomes, expenses]);
        }
    }
}
