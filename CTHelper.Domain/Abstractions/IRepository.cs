using System.Collections.ObjectModel;

namespace CTHelper.Domain.Abstractions
{
    public interface IRepository<T>
    {
        Task<T?> GetAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
        Task<TResult?> GetAsync<TResult>(ISpecification<T, TResult> spec, CancellationToken cancellationToken = default);
        Task<ReadOnlyCollection<T>> GetListAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
        Task<ReadOnlyCollection<TResult>> GetListAsync<TResult>(ISpecification<T, TResult> spec, CancellationToken cancellationToken = default);
        Task<int> CountAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteRangeAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
    }
}
