namespace ECommerce.Business.Models.Dtos.OrderItemStatuses
{
    public class OrderItemStatusListDto
    {
        public Guid Id { get; set; }
        public Entity.Enums.OrderItemStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
