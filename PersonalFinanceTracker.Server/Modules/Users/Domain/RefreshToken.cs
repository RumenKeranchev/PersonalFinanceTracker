namespace PersonalFinanceTracker.Server.Modules.Users.Domain
{
    public class RefreshToken
    {
        public Guid Id { get; set; }

        public string Token { get; set; } = null!;

        public DateTime ExpiresAt { get; set; }

        public string UserId { get; set; } = null!;
        public AppUser User { get; set; } = null!;
    }
}
