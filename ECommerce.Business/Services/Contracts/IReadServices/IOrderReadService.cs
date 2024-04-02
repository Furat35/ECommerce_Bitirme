using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Services.Contracts.IReadServices
{
    public interface IOrderReadService
    {
        public IReadRepository<Order> Orders { get; }
    }
}
