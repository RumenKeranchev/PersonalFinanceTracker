namespace PersonalFinanceTracker.Server.Modules.Users.Application
{
    public enum ClientType
    {
        Browser,
        Native
    }

    public static class ClientTypeExtensions
    {
        public static bool IsClientType(this IDictionary<object, object?> items, ClientType expected)
            => Enum.Parse<ClientType>(items["Client-Type"]!.ToString()!) == expected;
    }
}
