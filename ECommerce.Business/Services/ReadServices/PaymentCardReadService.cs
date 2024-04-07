using AutoMapper;
using ECommerce.Business.Extensions;
using ECommerce.Business.Models.Dtos.PaymentCards;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Exceptions;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Services.ReadServices
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
            ModelValidations.ThrowBadRequestIfIdIsNotValidGuid(userId);
            var paymentCard = await GetSinglePaymentCard(userId);
            return _mapper.Map<PaymentCardListDto>(paymentCard);
        }

        private async Task<PaymentCard> GetSinglePaymentCard(string userId)
        {
            ModelValidations.ThrowBadRequestIfIdIsNotValidGuid(userId);
            var paymentCard = await PaymentCards.GetSingleAsync(_ => _.Id.ToString() == userId);
            if (paymentCard is null)
                throw new NotFoundException("Kart bilgisi bulunamadı!");

            return paymentCard;
        }
    }
}
