namespace ECommerce.Business.Models.Dtos.PaymentCards
{
    public class PaymentCardListDto
    {
        public string Id { get; set; }
        public string NameSurname { get; set; }
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public string ExpireDate { get; set; }
    }
}
