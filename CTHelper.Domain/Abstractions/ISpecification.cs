using System.Linq.Expressions;

namespace CTHelper.Domain.Abstractions
{
    public interface    ISpecification<T>
    {
        Expression<Func<T, bool>>? Criteria { get; }
        IReadOnlyList<Expression<Func<T, object>>>? Includes { get; }
        IReadOnlyList<Expression<Func<T, object>>>? OrderBy { get; }
        IReadOnlyList<Expression<Func<T, object>>>? OrderByDescending { get; }

        int? Skip { get; }
        int? Take { get; }
        bool IsPagingEnabled { get; }

        bool AsNoTracking { get; }
        bool AsNoTrackingWithIdentityResolution { get; }
        bool AsSplitQuery { get; }
        bool IgnoreQueryFilters { get; }
    }

    public interface ISpecification<T, TResult> : ISpecification<T>
    {
        Expression<Func<T, TResult>> Selector { get; }
    }
}
