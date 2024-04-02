using AutoMapper;
using ECommerce.Business.Models.Dtos.PaymentCards;
using ECommerce.Business.Services.Contracts.IReadServices;
using ECommerce.Business.Services.Contracts.IWriteServices;
using ECommerce.Core.DataAccess.Repositories.Abstract;
using ECommerce.Core.Exceptions;
using ECommerce.Core.Extensions;
using ECommerce.DataAccess.UnitOfWorks;
using ECommerce.Entity.Entities;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Business.Services.WriteServices
{
    public class PaymentCardWriteService : IPaymentCardWriteService
    {
        private readonly IWriteRepository<PaymentCard> _paymentCardWriteRepository;
        private readonly IPaymentCardReadService _paymentCardReadService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IUserWriteService _userWriteService;
        private readonly IUserReadService _userReadService;

        public PaymentCardWriteService(IUnitOfWork unitOfWork, IMapper mapper, IPaymentCardReadService paymentCardReadService,
            IHttpContextAccessor httpContextAccessor, IUserWriteService userWriteService, IUserReadService userReadService)
        {
            _paymentCardWriteRepository = unitOfWork.GetWriteRepository<PaymentCard>();
            _mapper = mapper;
            _paymentCardReadService = paymentCardReadService;
            _httpContextAccessor = httpContextAccessor;
            _userWriteService = userWriteService;
            _userReadService = userReadService;
        }

        public async Task<PaymentCardListDto> AddPaymentCardAsync(PaymentCardAddDto paymentCard)
        {
            if (await CheckIfUserHasPaymentCard())
                throw new BadRequestException("Kart bilgisi mevcut!");

            var paymentCardToAdd = _mapper.Map<PaymentCard>(paymentCard);

            var activeUserId = _httpContextAccessor.HttpContext.User.GetActiveUserId();
            paymentCardToAdd.UserId = Guid.Parse(activeUserId);
            bool isAdded = await _paymentCardWriteRepository.AddAsync(paymentCardToAdd);
            if (!isAdded)
                throw new InternalServerErrorException();
            await UpdateUserPaymentCardId(paymentCardToAdd.Id);
            var paymentCardToList = _mapper.Map<PaymentCardListDto>(paymentCardToAdd);

            return paymentCardToList;
        }

        public async Task<bool> UpdatePaymentCardAsync(PaymentCardUpdateDto paymentCard)
        {
            var paymentCardToUpdate = await _paymentCardReadService.PaymentCards.GetByIdAsync(paymentCard.Id);
            if (paymentCardToUpdate is null)
                throw new NotFoundException("Kart bilgisi bulunamadı!");

            _mapper.Map(paymentCard, paymentCardToUpdate);
            return await _paymentCardWriteRepository.UpdateAsync(paymentCardToUpdate);
        }

        private async Task UpdateUserPaymentCardId(Guid paymentCardId)
        {
            var activeUserId = _httpContextAccessor.HttpContext.User.GetActiveUserId();
            var user = await _userReadService.Users.GetByIdAsync(activeUserId);
            user.PaymentCardId = paymentCardId;
            await _userWriteService.Users.UpdateAsync(user);
        }

        private async Task<bool> CheckIfUserHasPaymentCard()
        {
            var userId = _httpContextAccessor.HttpContext.User.GetActiveUserId();
            var paymentCard = await _paymentCardReadService.PaymentCards.GetSingleAsync(_ => _.UserId.ToString() == _httpContextAccessor.HttpContext.User.GetActiveUserId());

            return paymentCard != null ? true : false;
        }
    }
}
