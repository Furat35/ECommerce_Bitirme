using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Entities.Abstract;
using ECommerce.DataAccess.Repositories;
using ECommerce.DataAccess.Repositories.Contexts;
using Microsoft.EntityFrameworkCore.Storage;

namespace ECommerce.DataAccess.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EfECommerceContext _context;

        public UnitOfWork(EfECommerceContext context)
        {
            _context = context;
        }

        public async ValueTask DisposeAsync()
            => await _context.DisposeAsync();

        public IReadRepository<TEntity> GetReadRepository<TEntity>() where TEntity : class, IBaseEntity
            => new ReadRepository<TEntity>(_context);

        public IWriteRepository<TEntity> GetWriteRepository<TEntity>() where TEntity : class, IBaseEntity
            => new WriteRepository<TEntity>(_context);

        public async Task<IDbContextTransaction> BeginTransaction()
            => await _context.Database.BeginTransactionAsync();

        public async Task CommitTransaction()
          => await _context.Database.CommitTransactionAsync();

        public async Task RollbackTransaction()
          => await _context.Database.RollbackTransactionAsync();

        public int Save()
            => _context.SaveChanges();

        public async Task<int> SaveAsync()
            => await _context.SaveChangesAsync();


    }
}
