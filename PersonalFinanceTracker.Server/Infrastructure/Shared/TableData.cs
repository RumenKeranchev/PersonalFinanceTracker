namespace PersonalFinanceTracker.Server.Infrastructure.Shared
{
    using System.Text.Json.Serialization;

    public record TableData<T> where T : class
    {
        [JsonPropertyName("data")]
        public IEnumerable<T> Data { get; init; } = [];

        [JsonPropertyName("last_page")]
        public int LastPage { get; init; }
    }
}
