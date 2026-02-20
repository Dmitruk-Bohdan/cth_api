using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Common.Extensions;
using CTHelper.Domain.Entities;
using CTHelper.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace CTHelper.Persistence.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _entities;
        public EfRepository(AppDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _entities.AddAsync(entity);
            return entity;
        }

        public async Task<int> CountAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            var query = ApplySpecification(spec);
            return await query.CountAsync(cancellationToken);
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _entities.Remove(entity);
        }

        public async Task DeleteRangeAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            var entities = await ApplySpecification(spec).ToListAsync(cancellationToken);
            _entities.RemoveRange(entities);
        }

        public async Task<bool> ExistsAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return await ApplySpecification(spec)
                .AnyAsync(cancellationToken);
        }

        public async Task<T?> GetAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            var query = ApplySpecification(spec);
            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<TResult?> GetAsync<TResult>(ISpecification<T, TResult> spec, CancellationToken cancellationToken = default)
        {
            var query = ApplySpecification(spec);
            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<ReadOnlyCollection<T>> GetListAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            var result = await ApplySpecification(spec).ToListAsync(cancellationToken);
            return result.AsReadOnly();
        }

        public async Task<ReadOnlyCollection<TResult>> GetListAsync<TResult>(ISpecification<T, TResult> spec, CancellationToken cancellationToken = default)
        {
            var result = await ApplySpecification(spec).ToListAsync(cancellationToken);
            return result.AsReadOnly();
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _entities.Update(entity);
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            IQueryable<T> query = _entities.AsQueryable();

            if (spec.IgnoreQueryFilters)
                query = query.IgnoreQueryFilters();

            if (spec.AsNoTracking)
                query = query.AsNoTracking();

            if(spec.AsNoTrackingWithIdentityResolution)
                query = query.AsNoTrackingWithIdentityResolution();

            if(spec.AsSplitQuery)
                query = query.AsSplitQuery();

            if(spec.IsPagingEnabled)
            {
                if (spec.Skip.HasValue)
                    query = query.Skip(spec.Skip.Value);

                if (spec.Take.HasValue)
                    query = query.Take(spec.Take.Value);
            }


            if (spec.Criteria != null)
                query = query.Where(spec.Criteria);

            if (spec.Includes != null)
            {
                foreach (var include in spec.Includes)
                    query = query.Include(include);
            }

            if (!spec.OrderBy.IsNullOrEmpty())
            {
                IOrderedQueryable<T>? orderedQuery = null;

                for (int i = 0; i < spec.OrderBy!.Count; i++)
                {
                    var order = spec.OrderBy[i];

                    orderedQuery = i == 0
                        ? query.OrderBy(order)
                        : orderedQuery!.ThenBy(order);
                }

                query = orderedQuery!;
            }
            else if(!spec.OrderByDescending.IsNullOrEmpty())
            {
                IOrderedQueryable<T>? orderedQuery = null;

                for (int i = 0; i < spec.OrderByDescending!.Count; i++)
                {
                    var order = spec.OrderByDescending[i];

                    orderedQuery = i == 0
                        ? query.OrderByDescending(order)
                        : orderedQuery!.ThenByDescending(order);
                }

                query = orderedQuery!;
            }

            if (spec.Includes != null)
            {
                foreach (var include in spec.Includes)
                    query = query.Include(include);
            }

            return query;
        }
        private IQueryable<TResult> ApplySpecification<TResult>(
            ISpecification<T, TResult> spec)
        {
            var query = ApplySpecification((ISpecification<T>)spec);

            return query.Select(spec.Selector);
        }
    }
}
