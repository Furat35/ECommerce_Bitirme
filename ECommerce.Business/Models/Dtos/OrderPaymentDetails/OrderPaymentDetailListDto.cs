namespace ECommerce.Business.Models.Dtos.OrderPaymentDetails
{
    public class OrderPaymentDetailListDto
    {
        public Guid Id { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
