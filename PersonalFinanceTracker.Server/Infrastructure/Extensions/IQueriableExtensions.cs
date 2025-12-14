namespace PersonalFinanceTracker.Server.Infrastructure.Extensions
{
    using PersonalFinanceTracker.Server.Infrastructure.Requests;

    public static class IQueriableExtensions
    {
        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> queryable, PagedQuery pagedQuery)
            => queryable.Skip(pagedQuery.Index * pagedQuery.Size).Take(pagedQuery.Size);
    }
}
