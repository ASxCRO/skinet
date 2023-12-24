using System.Linq.Expressions;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
       Task<IReadOnlyList<T>> ListAllAsync();
        Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> criteria);
        Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[] includes);
        Task<IReadOnlyList<T>> ListPaginatedAsync(int page, int pageSize);
        Task<IReadOnlyList<T>> ListPaginatedAsync(Expression<Func<T, bool>> criteria, int page, int pageSize);
        Task<IReadOnlyList<T>> ListPaginatedAsync(Expression<Func<T, bool>> criteria, Expression<Func<T, object>> orderBy, string sortOrder, int page, int pageSize, params Expression<Func<T, object>>[] includes);
        Task<T> GetByIdAsync(object id);
        Task<T> GetByIdAsync(object id, params Expression<Func<T, object>>[] includes);
    }
}