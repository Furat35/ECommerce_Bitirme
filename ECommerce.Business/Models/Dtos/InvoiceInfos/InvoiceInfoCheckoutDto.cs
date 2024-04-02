namespace ECommerce.Business.Models.Dtos.InvoiceInfos
{
    public class InvoiceInfoCheckoutDto
    {
        public string CompanyName { get; set; }
        public string CompanyLogo { get; set; }
        public string CompanyPhone { get; set; }
        public string ClientFullName { get; set; }
        public string ClientPhone { get; set; }
        public float TotalPrice { get; set; }
    }
}
