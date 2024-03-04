using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Entities.Abstract;

namespace ECommerce.DataAccess.UnitOfWorks
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IReadRepository<TEntity> GetReadRepository<TEntity>() where TEntity : class, IBaseEntity;
        IWriteRepository<TEntity> GetWriteRepository<TEntity>() where TEntity : class, IBaseEntity;
        Task<int> SaveAsync();
        int Save();
    }
}
