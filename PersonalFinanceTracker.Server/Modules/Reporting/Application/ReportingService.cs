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

        public async Task<Result<PointDashboard>> GetDashboardAsync()
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
                .Select(t => t.Date.ToString("MMMM yyyy"))
                .Distinct()
                .ToList();

            var incomes = labels
                .Select(l => new DatasetPoint
                (
                    CultureInfo.InvariantCulture.TextInfo.ToTitleCase(l),
                    transactions
                        .Where(t => t.Type == TransactionType.Income && t.Date.ToString("MMM yyyy") == l)
                        .Select(t => t.Amount)
                        .DefaultIfEmpty()
                        .Sum()
                ))
                .ToList();

            var incomeDashboard = new DashboardPointDataset("Income", incomes);

            var expenses = labels
                 .Select(l => new DatasetPoint
                 (
                     CultureInfo.InvariantCulture.TextInfo.ToTitleCase(l),
                     transactions
                         .Where(t => t.Type == TransactionType.Expense && t.Date.ToString("MMM yyyy") == l)
                         .Select(t => t.Amount)
                         .DefaultIfEmpty()
                         .Sum()
                 ))
                 .ToList();

            var expensesDashboard = new DashboardPointDataset("Expense", expenses);

            return new PointDashboard([incomeDashboard, expensesDashboard]);
        }
    }
}
