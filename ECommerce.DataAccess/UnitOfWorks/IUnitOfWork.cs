using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Entities.Abstract;
using Microsoft.EntityFrameworkCore.Storage;

namespace ECommerce.DataAccess.UnitOfWorks
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IReadRepository<TEntity> GetReadRepository<TEntity>() where TEntity : class, IBaseEntity;
        IWriteRepository<TEntity> GetWriteRepository<TEntity>() where TEntity : class, IBaseEntity;
        Task<IDbContextTransaction> BeginTransaction();
        Task CommitTransaction();
        Task RollbackTransaction();
        Task<int> SaveAsync();
        int Save();
    }
}
