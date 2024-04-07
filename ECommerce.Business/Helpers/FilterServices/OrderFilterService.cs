using AutoMapper;
using ECommerce.Business.Helpers.FilterServices.Orders;
using ECommerce.Business.Helpers.Products;
using ECommerce.Business.Models.Dtos.Orders;
using ECommerce.Core.Filters;
using ECommerce.Core.ResponseHeaders;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Helpers.FilterServices
{
    public class OrderFilterService
    {
        private readonly IMapper _mapper;
        private List<Order> _orders;

        public OrderFilterService(IMapper mapper, List<Order> orders)
        {
            _orders = orders;
            _mapper = mapper;
        }

        public ProductResponse<List<OrderListDto>> FilterOrders(OrderRequestFilter filters)
        {
            int pageNumber = _orders.Count() % filters.PageSize == 0 ? _orders.Count() / filters.PageSize : _orders.Count() / filters.PageSize + 1;
            Metadata metadata = new(filters.Page, filters.PageSize, _orders.Count(), pageNumber);
            _orders = AddPagination(filters);
            var header = new CustomHeaders().AddPaginationHeader(metadata);

            return new()
            {
                ResponseValue = _mapper.Map<List<OrderListDto>>(_orders),
                Headers = header
            };
        }
        private List<Order> AddPagination(OrderRequestFilter filters)
       => _orders
           .OrderByDescending(_ => _.CreatedDate)
           .Skip((filters.Page - 1) * filters.PageSize)
           .Take(filters.PageSize)
           .ToList();
    }
}
