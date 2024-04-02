using ECommerce.Core.Entities.Abstract;

namespace ECommerce.Core.DataAccess.Repositories.Abstract
{
    public interface IWriteRepository<TEntity>
        where TEntity : IBaseEntity
    {
        Task<bool> AddAsync(TEntity model);
        Task<bool> AddRangeAsync(List<TEntity> models);
        Task<bool> SafeRemoveAsync(TEntity model);
        Task<bool> RemoveAsync(TEntity model);
        Task<bool> RemoveRangeAsync(List<TEntity> models);
        Task<bool> UpdateAsync(TEntity model);
        Task<bool> SaveChangesAsync();
    }

}
