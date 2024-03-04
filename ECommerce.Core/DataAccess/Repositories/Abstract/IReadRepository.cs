using ECommerce.Core.Entities.Abstract;
using System.Linq.Expressions;

namespace ECommerce.Core.DataAccess.Repositories.Abstract
{
    public interface IReadRepository<TEntity>
        where TEntity : IBaseEntity
    {
        IQueryable<TEntity> GetAll(bool tracking = true, params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> predicate, bool tracking = true, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = true, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<TEntity> GetByIdAsync(string id, bool tracking = true, params Expression<Func<TEntity, object>>[] includeProperties);
    }
}
