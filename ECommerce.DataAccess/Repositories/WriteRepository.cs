using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Entities.Abstract;
using ECommerce.DataAccess.Repositories.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.DataAccess.Repositories
{
    public class WriteRepository<TEntity> : IWriteRepository<TEntity>
        where TEntity : class, IBaseEntity
    {
        private readonly EfECommerceContext _context;
        public WriteRepository(EfECommerceContext context)
        {
            _context = context;
        }

        public DbSet<TEntity> Table => _context.Set<TEntity>();

        public async Task<bool> AddAsync(TEntity model)
        {
            await Table.AddAsync(model);
            return await SaveChangesAsync();
        }

        public async Task<bool> AddRangeAsync(List<TEntity> models)
        {
            await Table.AddRangeAsync(models);
            return await SaveChangesAsync();
        }

        public async Task<bool> SafeRemoveAsync(TEntity model)
        {
            model.DeletedDate = DateTime.UtcNow;
            model.IsValid = false;
            Table.Update(model);
            return await SaveChangesAsync();
        }

        public async Task<bool> RemoveAsync(TEntity model)
        {
            Table.Remove(model);
            return await SaveChangesAsync();
        }

        public async Task<bool> RemoveRangeAsync(List<TEntity> models)
        {
            Table.RemoveRange(models);
            return await SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(TEntity model)
        {
            Table.Update(model);
            return await SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
            => await _context.SaveChangesAsync() != 0;
    }
}
