namespace ECommerce.Business.Models.Dtos.PaymentCards
{
    public class PaymentCardUpdateDto
    {
        public string Id { get; set; }
        public string NameSurname { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
