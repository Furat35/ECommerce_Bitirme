using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Entities.Abstract;
using ECommerce.DataAccess.Repositories.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
            EntityEntry<TEntity> entry = await Table.AddAsync(model);
            return entry.State == EntityState.Added;
        }

        public async Task<bool> AddRangeAsync(List<TEntity> models)
        {
            await Table.AddRangeAsync(models);
            return true;
        }

        public bool SafeRemove(TEntity model)
        {
            model.IsValid = false;
            EntityEntry<TEntity> entry = Table.Update(model);
            return entry.State == EntityState.Deleted;
        }

        public bool Update(TEntity model)
        {
            EntityEntry<TEntity> entry = Table.Update(model);
            return entry.State == EntityState.Modified;
        }

        public async Task<int> SaveAsync()
            => await _context.SaveChangesAsync();
    }
}
