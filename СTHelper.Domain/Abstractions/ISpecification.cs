using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace СTHelper.Domain.Abstractions
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>>? Criteria { get; }
        IReadOnlyList<Expression<Func<T, object>>>? Includes { get; }
        IReadOnlyList<Expression<Func<T, object>>>? OrderBy { get; }
        IReadOnlyList<Expression<Func<T, object>>>? OrderByDescending { get; }

        int? Skip { get; }
        int? Take { get; }
        bool IsPagingEnabled { get; }

        bool AsNoTracking { get; }
        bool AsSplitQuery { get; }
        bool IgnoreQueryFilters { get; }
    }

    public interface ISpecification<T, TResult> : ISpecification<T>
    {
        Expression<Func<T, TResult>> Selector { get; }
    }
}
