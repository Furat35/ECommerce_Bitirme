using AutoMapper;
using ECommerce.Business.Helpers.FilterServices.OrderItems;
using ECommerce.Business.Helpers.Products;
using ECommerce.Business.Models.Dtos.OrderItems;
using ECommerce.Core.Filters;
using ECommerce.Core.ResponseHeaders;
using ECommerce.Entity.Entities;

namespace ECommerce.Business.Helpers.FilterServices
{
    public class OrderItemFilterService
    {
        private readonly IMapper _mapper;
        private List<OrderItem> _orderItems;

        public OrderItemFilterService(IMapper mapper, List<OrderItem> orderItems)
        {
            _mapper = mapper;
            _orderItems = orderItems;
        }

        public ProductResponse<List<OrderItemListDto>> FilterOrderItems(OrderItemRequestFilter filters)
        {
            int pageNumber = _orderItems.Count() % filters.PageSize == 0 ? _orderItems.Count() / filters.PageSize : _orderItems.Count() / filters.PageSize + 1;
            Metadata metadata = new(filters.Page, filters.PageSize, _orderItems.Count(), pageNumber);
            _orderItems = AddPagination(filters);
            var header = new CustomHeaders().AddPaginationHeader(metadata);

            return new()
            {
                ResponseValue = _mapper.Map<List<OrderItemListDto>>(_orderItems),
                Headers = header
            };
        }

        private List<OrderItem> AddPagination(OrderItemRequestFilter filters)
          => _orderItems
              .OrderByDescending(_ => _.CreatedDate)
              .Skip((filters.Page - 1) * filters.PageSize)
              .Take(filters.PageSize)
              .ToList();
    }
}
