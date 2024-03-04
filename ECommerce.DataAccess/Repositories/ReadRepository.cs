using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Entities.Abstract;
using ECommerce.DataAccess.Repositories.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ECommerce.DataAccess.Repositories
{
    public class ReadRepository<TEntity> : IReadRepository<TEntity> where TEntity : class, IBaseEntity
    {
        private readonly EfECommerceContext _context;

        public ReadRepository(EfECommerceContext context)
        {
            _context = context;
        }

        public DbSet<TEntity> Table => _context.Set<TEntity>();

        public IQueryable<TEntity> GetAll(bool tracking = false, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = SaveTrackingStatus(tracking);
            if (includeProperties.Any())
                foreach (var item in includeProperties)
                    query = query.Include(item);

            return query;
        }

        public IQueryable<TEntity> GetWhere(Expression<Func<TEntity, bool>> predicate, bool tracking = false, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = SaveTrackingStatus(tracking);
            if (includeProperties.Any())
                foreach (var item in includeProperties)
                    query = query.Include(item);

            return query.Where(predicate);
        }

        public async Task<TEntity> GetByIdAsync(string id, bool tracking = true, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = SaveTrackingStatus(tracking);
            if (includeProperties.Any())
                foreach (var item in includeProperties)
                    query = query.Include(item);

            return await query.FirstOrDefaultAsync(_ => _.Id == Guid.Parse(id));
        }


        public async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate, bool tracking = true, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = SaveTrackingStatus(tracking);
            if (includeProperties.Any())
                foreach (var item in includeProperties)
                    query = query.Include(item);

            return await query.FirstOrDefaultAsync(predicate);
        }

        private IQueryable<TEntity> SaveTrackingStatus(bool tracking)
        {
            var query = Table.AsQueryable();
            if (!tracking)
                query = query.AsNoTracking();

            return query;
        }
    }
}
