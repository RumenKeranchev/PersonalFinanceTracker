namespace PersonalFinanceTracker.Server.Modules.Users.Application
{
    using System.Net;
    using static Resourses.Exceptions;

    public static class UsersErrors
    {
        public static Error UserAlreadyExists => new("users.auth.user_exists", EmailAldreadyExists, HttpStatusCode.Conflict);

        public static Error InvalidCredentials => new("users.auth.invalid_credentials", InvalidEmailOrPassword, HttpStatusCode.Unauthorized);
    }
}
