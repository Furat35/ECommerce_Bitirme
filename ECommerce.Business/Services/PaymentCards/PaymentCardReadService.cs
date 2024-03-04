using AutoMapper;
using ECommerce.Business.Models.Dtos.PaymentCards;
using ECommerce.Business.Services.PaymentCards.Abstract;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Exceptions;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Services.PaymentCards
{
    public class PaymentCardReadService : IPaymentCardReadService
    {
        private readonly IMapper _mapper;
        public PaymentCardReadService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            PaymentCards = unitOfWork.GetReadRepository<PaymentCard>();
            _mapper = mapper;
        }

        public IReadRepository<PaymentCard> PaymentCards { get; }

        public async Task<PaymentCardListDto> GetPaymentCardByUserIdAsync(string userId)
        {
            var paymentCard = await PaymentCards.GetSingleAsync(_ => _.UserId.ToString() == userId);
            if (paymentCard is null || !paymentCard.IsValid)
                throw new NotFoundException("Kart bilgisi bulunamadı!");

            return _mapper.Map<PaymentCardListDto>(paymentCard);
        }
    }
}
