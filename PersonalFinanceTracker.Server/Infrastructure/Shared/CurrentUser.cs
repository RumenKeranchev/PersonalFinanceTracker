namespace PersonalFinanceTracker.Server.Infrastructure.Shared
{
    using Modules.Users.Application;
    using Resourses;

    public interface ICurrentUser
    {
        Guid Id { get; }
        bool IsAdmin { get; }
        //bool IsAuthenticated { get; }
    }

    public sealed class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid Id => Guid.TryParse(_httpContextAccessor.HttpContext?.User.Identity?.Name, out var id) ? id : throw new UnauthorizedAccessException(Exceptions.InvalidId);

        public bool IsAdmin => _httpContextAccessor.HttpContext?.User.IsInRole(Roles.Admin) ?? false;

        //public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    }
}
