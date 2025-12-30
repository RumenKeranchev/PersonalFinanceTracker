namespace PersonalFinanceTracker.Server.Modules.Users.Application
{
    public static class UsersErrors
    {
        public static Error UserAlreadyExists => new("users.auth.user_exists", "User with this email already exists.");

        public static Error InvalidCredentials => new("users.auth.invalid_credentials", "Invalid email or password.");
    }
}
