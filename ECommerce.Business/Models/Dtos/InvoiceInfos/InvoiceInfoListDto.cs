namespace ECommerce.Business.Models.Dtos.InvoiceInfos
{
    public class InvoiceInfoListDto
    {
        public Guid Id { get; set; }
        public string ClientFullName { get; set; }
        public string ClientPhone { get; set; }
        public float TotalPrice { get; set; }
    }
}
