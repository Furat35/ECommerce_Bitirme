using ECommerce.Business.Models.Dtos.PaymentCards;

namespace ECommerce.Business.Services.Contracts.IWriteServices
{
    public interface IPaymentCardWriteService
    {
        Task<PaymentCardListDto> AddPaymentCardAsync(PaymentCardAddDto paymentCard);
        Task<bool> UpdatePaymentCardAsync(PaymentCardUpdateDto paymentCard);
    }
}
