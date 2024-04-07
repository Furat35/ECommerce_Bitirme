using ECommerce.Business.Models.Dtos.OrderPaymentDetails;
using ECommerce.Business.Models.Dtos.ShippingPlaces;

namespace ECommerce.Business.Models.Dtos.Orders
{
    public class OrderCheckoutDto
    {
        public bool UseSavedUserAddress { get; set; } = true;
        public bool UseSavedPaymentCard { get; set; } = true;
        public ShippingPlaceCheckoutDto ShippingPlace { get; set; }
        public OrderPaymentDetailCheckoutDto OrderPaymentDetail { get; set; }
    }
}
