using System.Collections.ObjectModel;

namespace СTHelper.Domain.Abstractions
{
    public interface IRepository<T>
    {
        Task<T>? GetAsync(ISpecification<T> spec);
        Task<ReadOnlyCollection<T>>? GetListAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);
        Task<T>? AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<bool> ExistsAsync(ISpecification<T> spec);
    }
}
