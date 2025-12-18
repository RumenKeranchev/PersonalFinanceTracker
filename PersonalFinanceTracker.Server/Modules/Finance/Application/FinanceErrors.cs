namespace PersonalFinanceTracker.Server.Modules.Finance.Application
{
    using Infrastructure.Shared;

    public static class FinanceErrors
    {
        public static Error TransactionNotFound => new("finance.transaction.not_found", "Transaction not found!");

        public static Error InvalidAmount(decimal amount) => new("finance.transaction.invalid_amount", $"Invalid amount [{amount}]!");
    }
}
