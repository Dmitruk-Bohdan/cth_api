using CTHelper.Domain.Entities;

namespace CTHelper.Infrastructure.Extensions
{
    public static class QueryableExtension
    {
        public static IQueryable<TestAttempt> ApplyDateFilter(this IQueryable<TestAttempt> query, DateTimeOffset? dateFrom, DateTimeOffset? dateTo)
        {
            if (dateFrom.HasValue)
                query = query.Where(ta => ta.CreatedAt >= dateFrom.Value);
            if (dateTo.HasValue)
                query = query.Where(ta => ta.CreatedAt <= dateTo.Value);
            return query;
        }

        public static IQueryable<UserAnswer> ApplyDateFilter(this IQueryable<UserAnswer> query, DateTimeOffset? dateFrom, DateTimeOffset? dateTo)
        {
            if (dateFrom.HasValue)
                query = query.Where(ua => ua.TestAttempt.CreatedAt >= dateFrom.Value);
            if (dateTo.HasValue)
                query = query.Where(ua => ua.TestAttempt.CreatedAt <= dateTo.Value);
            return query;
        }
    }
}
