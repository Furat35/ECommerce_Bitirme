using ECommerce.Business.Models.Dtos.OrderItemProduct;
using ECommerce.Business.Models.Dtos.OrderItemStatuses;

namespace ECommerce.Business.Models.Dtos.OrderItems
{
    public class OrderItemListDto
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public OrderItemProductListDto Product { get; set; }
        public ICollection<OrderItemStatusListDto> OrderItemStatuses { get; set; } = new List<OrderItemStatusListDto>();
    }
}
