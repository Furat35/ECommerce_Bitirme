namespace ECommerce.Business.Models.Dtos.OrderPaymentDetails
{
    public class OrderPaymentDetailCheckoutDto
    {
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
