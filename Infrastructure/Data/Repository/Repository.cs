using System.Linq.Expressions;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _storeContext;

        public Repository(StoreContext storeContext)
        {
            this._storeContext = storeContext;
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _storeContext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> criteria)
        {
            return await _storeContext.Set<T>().Where(criteria).ToListAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[] includes)
        {
            var query = _storeContext.Set<T>().Where(criteria);
            query = ApplyIncludes(query, includes);
            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> ListPaginatedAsync(int page, int pageSize)
        {
            var query = _storeContext.Set<T>().Skip((page - 1) * pageSize).Take(pageSize);
            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> ListPaginatedAsync(Expression<Func<T, bool>> criteria, int page, int pageSize)
        {
            var query = _storeContext.Set<T>().Where(criteria).Skip((page - 1) * pageSize).Take(pageSize);
            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> ListPaginatedAsync(Expression<Func<T, bool>> criteria, Expression<Func<T, object>> orderBy, string sortOrder, int page, int pageSize, params Expression<Func<T, object>>[] includes)
        {
            var query = _storeContext.Set<T>().Where(criteria);
            
            if(sortOrder == "asc")
            {
                query = query.OrderBy(orderBy);
            }
            else{
                query = query.OrderByDescending(orderBy);
            }
            
            
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            query = ApplyIncludes(query, includes);
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _storeContext.Set<T>().FindAsync(id);
        }

        public async Task<T> GetByIdAsync(object id, params Expression<Func<T, object>>[] includes)
        {
            var entity = await _storeContext.Set<T>().FindAsync(id);
            if (entity != null)
            {
                var entry = _storeContext.Entry(entity);
                ApplyIncludes(entry, includes);
            }
            return entity;
        }

        private IQueryable<T> ApplyIncludes(IQueryable<T> query, params Expression<Func<T, object>>[] includes)
        {
            return includes.Aggregate(query, (current, include) => current.Include(include));
        }

        private void ApplyIncludes(EntityEntry<T> entry, params Expression<Func<T, object>>[] includes)
        {
            foreach (var include in includes)
            {
                entry.Reference(include).Load();
            }
        }
    }
}