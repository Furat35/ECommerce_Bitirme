using ECommerce.Business.Models.Dtos.InvoiceInfos;
using ECommerce.Business.Models.Dtos.OrderItems;
using ECommerce.Business.Models.Dtos.OrderPaymentDetails;
using ECommerce.Business.Models.Dtos.OrderStatuses;
using ECommerce.Business.Models.Dtos.ShippingPlaces;
using ECommerce.Business.Models.Dtos.Users;

namespace ECommerce.Business.Models.Dtos.Orders
{
    public class OrderListDto
    {
        public Guid Id { get; set; }
        public float TotalPrice { get; set; }
        public UserListDto User { get; set; }
        public ShippingPlaceListDto ShippingPlace { get; set; }
        public InvoiceInfoListDto InvoiceInfo { get; set; }
        public OrderPaymentDetailListDto OrderPaymentDetail { get; set; }
        public ICollection<OrderItemListDto> OrderItems { get; set; } = new List<OrderItemListDto>();
        public ICollection<OrderStatusListDto> OrderStatuses { get; set; } = new List<OrderStatusListDto>();
        public DateTime CreatedDate { get; set; }
    }
}
