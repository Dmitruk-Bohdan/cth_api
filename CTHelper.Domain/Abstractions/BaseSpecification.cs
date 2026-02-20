using System.Linq.Expressions;
using CTHelper.Domain.Common.Extensions;

namespace CTHelper.Domain.Abstractions 
{
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>>? Criteria { get; protected set; }

        private List<Expression<Func<T, object>>> _includes = new();

        public IReadOnlyList<Expression<Func<T, object>>> Includes => _includes;

        private List<Expression<Func<T, object>>> _orderBy = new();
        public IReadOnlyList<Expression<Func<T, object>>> OrderBy => _orderBy;
        
        private List<Expression<Func<T, object>>> _orderByDescending = new();
        public IReadOnlyList<Expression<Func<T, object>>> OrderByDescending => _orderByDescending;

        public int? Skip { get; protected set; }
        public int? Take { get; protected set; }
        public bool IsPagingEnabled { get; protected set; }

        public bool AsNoTracking { get; protected set; }
        public bool AsNoTrackingWithIdentityResolution { get; protected set; }
        public bool AsSplitQuery { get; protected set; }
        public bool IgnoreQueryFilters { get; protected set; }

        protected void AddCriteria(Expression<Func<T, bool>> criteria)
        {
            Criteria = Criteria == null
                ? criteria
                : Criteria.And(criteria);
        }

        protected void AddOrderBy(Expression<Func<T, object>> orderBy)
            => _orderBy.Add(orderBy);

        protected void AddOrderBy(IReadOnlyCollection<Expression<Func<T, object>>> orderByCollection)
            => _orderBy.AddRange(orderByCollection);
        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDesc)
            => _orderByDescending.Add(orderByDesc);
        protected void AddOrderByDescending(IReadOnlyCollection<Expression<Func<T, object>>> orderByDescCollection)
            => _orderByDescending.AddRange(orderByDescCollection);

        protected void AddPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }

        protected void EnableAsNoTracking()
            => AsNoTracking = true;

        protected void EnableAsNoTrackingWithIdentityResolution()
            => AsNoTrackingWithIdentityResolution = true;

        protected void EnableAsSplitQuery()
            => AsSplitQuery = true;

        protected void EnableIgnoreQueryFilters()
            => IgnoreQueryFilters = true;
    }

    public abstract class BaseSpecification<T, TResult> : BaseSpecification<T>, ISpecification<T, TResult>
    {
        private Expression<Func<T, TResult>>? _selector;
        public Expression<Func<T, TResult>> Selector
            => _selector ?? throw new InvalidOperationException("Selector not set");

        protected void ApplySelector(Expression<Func<T, TResult>> selector)
            => _selector = selector;

    }
}
