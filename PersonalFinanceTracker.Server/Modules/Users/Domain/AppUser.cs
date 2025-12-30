namespace PersonalFinanceTracker.Server.Modules.Users.Domain
{
    using Microsoft.AspNetCore.Identity;

    public class AppUser : IdentityUser
    {
        public AppUser(string email, string username) : base()
        {
            Email = email;
            UserName = username;
        }
    }
}
