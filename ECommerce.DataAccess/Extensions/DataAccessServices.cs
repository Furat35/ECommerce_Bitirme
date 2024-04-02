using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.DataAccess.Repositories;
using ECommerce.DataAccess.Repositories.Contexts;
using ECommerce.DataAccess.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.DataAccess.Extensions
{
    public static class DataAccessServices
    {
        public static async Task<IServiceCollection> AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Entityframework konfigürasyonları
            services.AddDbContext<EfECommerceContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("ECommerceMssql")));

            // Scoped servisler
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));
            services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
            services.AddSingleton<ContextSeed>();

            return services;
        }
    }
}
