using ECommerce.Core.Entities.Abstract;

namespace ECommerce.Core.DataAccess.Repositories.Abstract
{
    public interface IWriteRepository<TEntity>
        where TEntity : IBaseEntity
    {
        Task<bool> AddAsync(TEntity model);
        Task<bool> AddRangeAsync(List<TEntity> models);
        bool SafeRemove(TEntity model);
        bool Update(TEntity model);
        Task<int> SaveAsync();
    }

}
