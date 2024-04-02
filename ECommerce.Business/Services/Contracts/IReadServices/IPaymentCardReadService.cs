using ECommerce.Business.Models.Dtos.PaymentCards;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Services.Contracts.IReadServices
{
    public interface IPaymentCardReadService
    {
        Task<PaymentCardListDto> GetPaymentCardByUserIdAsync(string paymentCardId);
        IReadRepository<PaymentCard> PaymentCards { get; }
    }
}
