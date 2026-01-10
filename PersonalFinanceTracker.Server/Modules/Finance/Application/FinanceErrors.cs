namespace PersonalFinanceTracker.Server.Modules.Finance.Application
{
    using Infrastructure.Shared;
    using Resourses;

    public static class FinanceErrors
    {
        public static Error TransactionNotFound => new("finance.transaction.not_found", Exceptions.TransactionNotFound);
    }
}
