namespace PersonalFinanceTracker.Server.Modules.Users.Domain
{
    public class RefreshToken
    {
        public RefreshToken()
        {

        }

        public RefreshToken(string token, int expireDays, string userId)
        {
            Token = token;
            ExpiresAt = DateTime.UtcNow.AddDays(expireDays);
            UserId = userId;
            DeviceId = Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        public string Token { get; init; } = null!;

        public DateTime ExpiresAt { get; init; }

        public string UserId { get; init; } = null!;
        public AppUser User { get; private set; } = null!;

        public Guid DeviceId { get; init; }
    }
}
