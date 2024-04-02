using AutoMapper;
using ECommerce.Business.Models.Dtos.CartItems;
using ECommerce.Business.Models.Dtos.Orders;
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
    public class OrderWriteService : IOrderWriteService
    {
        private readonly IWriteRepository<Order> _orderWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICartReadService _cartReadService;
        private readonly ICartWriteService _cartWriteService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductWriteService _productWriteService;
        private readonly IAddressReadService _addressReadService;
        private readonly IOrderReadService _orderReadService;

        public OrderWriteService(IUnitOfWork unitOfWork, IMapper mapper, ICartReadService cartReadService, IHttpContextAccessor httpContextAccessor,
            ICartWriteService cartWriteService, IProductWriteService productWriteService, IAddressReadService addressReadService, IOrderReadService orderReadService)
        {
            _orderWriteRepository = unitOfWork.GetWriteRepository<Order>();
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cartReadService = cartReadService;
            _httpContextAccessor = httpContextAccessor;
            _cartWriteService = cartWriteService;
            _productWriteService = productWriteService;
            _addressReadService = addressReadService;
            _orderReadService = orderReadService;
        }

        public async Task CheckoutOrder(OrderCheckoutDto orderCheckoutDto)
        {
            using (var transaction = await _unitOfWork.BeginTransaction())
            {
                try
                {
                    var orderCheckout = await CreateOrder(orderCheckoutDto);
                    bool orderIsCreated = await _orderWriteRepository.AddAsync(orderCheckout);
                    if (orderIsCreated)
                        await _cartWriteService.ClearCart(_httpContextAccessor.HttpContext.User.GetActiveUserId());
                    else
                        throw new InternalServerErrorException();

                    await _unitOfWork.CommitTransaction();
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransaction();
                    throw new BadRequestException(ex.Message);
                }
            }
        }

        public async Task<bool> ChangeOrderStatus(Entity.Enums.OrderStatus orderStatus, string orderId)
        {
            var order = await _orderReadService.Orders.GetSingleAsync(_ => _.Id == Guid.Parse(orderId), true, [_ => _.OrderStatuses]);
            foreach (var status in order.OrderStatuses)
                status.IsValid = false;
            order.OrderStatuses.Add(new OrderStatus { Status = orderStatus, IsValid = true });
            bool isUpdated = await _orderWriteRepository.UpdateAsync(order);

            return isUpdated;
        }

        private async Task<Order> CreateOrder(OrderCheckoutDto orderCheckoutDto)
        {
            var userId = _httpContextAccessor.HttpContext.User.GetActiveUserId();
            var cart = await _cartReadService.GetCartAsync(userId);

            await UpdateProductStocks(cart.CartItems);
            var orderCheckout = _mapper.Map<Order>(orderCheckoutDto);
            orderCheckout.TotalPrice = cart.TotalPrice;
            orderCheckout.OrderStatuses = new List<OrderStatus>() { new OrderStatus { Status = Entity.Enums.OrderStatus.Pending, IsValid = true } };
            await SetOrderShippingPlace(orderCheckout, orderCheckoutDto.UseActiveUserAddress);
            SetOrderInvoiceInfo(orderCheckout, cart.TotalPrice);
            SetOrderItemStatusesToPending(orderCheckout, cart.CartItems);
            orderCheckout.UserId = Guid.Parse(userId);

            return orderCheckout;
        }

        private async Task UpdateProductStocks(ICollection<CartItemListDto> cartItems)
        {
            foreach (var cartItem in cartItems)
                await _productWriteService.DecreaseProductQuantity(cartItem.Product.Id.ToString(), cartItem.Quantity);
        }

        private async Task SetOrderShippingPlace(Order orderCheckout, bool useActiveUserAddress)
        {
            var userId = _httpContextAccessor.HttpContext.User.GetActiveUserId();
            if (!useActiveUserAddress)
                orderCheckout.ShippingPlace = _mapper.Map<ShippingPlace>(orderCheckout.ShippingPlace);
            else
            {
                var userAddress = await _addressReadService.GetUserAddress(userId);
                if (userAddress != null)
                    orderCheckout.ShippingPlace = _mapper.Map<ShippingPlace>(userAddress);
            }
        }

        private void SetOrderInvoiceInfo(Order order, float totalPrice)
            => order.InvoiceInfo = new InvoiceInfo
            {
                ClientFullName = _httpContextAccessor.HttpContext.User.GetActiveUserFullName(),
                ClientPhone = _httpContextAccessor.HttpContext.User.GetActivePhone(),
                TotalPrice = totalPrice,
            };

        private void SetOrderItemStatusesToPending(Order order, ICollection<CartItemListDto> cartItems)
        {
            order.OrderItems = _mapper.Map<List<OrderItem>>(cartItems.ToList());
            foreach (var orderItem in order.OrderItems)
                orderItem.OrderItemStatuses.Add(new OrderItemStatus { Status = Entity.Enums.OrderItemStatus.Pending, IsValid = true });
        }
    }
}
