namespace PersonalFinanceTracker.Server.Infrastructure.Shared
{
    using Newtonsoft.Json;

    public record TableData<T>(IEnumerable<T> Data, [JsonProperty("last_page")] int LastPage) where T : class;
}
