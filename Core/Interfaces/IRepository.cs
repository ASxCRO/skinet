using System.Linq.Expressions;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> criteria);
        Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[] includes);
        Task<T> GetByIdAsync(object id);
        Task<T> GetByIdAsync(object id, params Expression<Func<T, object>>[] includes);
    }
}