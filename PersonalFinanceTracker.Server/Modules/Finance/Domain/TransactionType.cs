namespace PersonalFinanceTracker.Server.Modules.Finance.Domain
{
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TransactionType
    {
        Income,
        Expense,
        Transfer
    }
}
