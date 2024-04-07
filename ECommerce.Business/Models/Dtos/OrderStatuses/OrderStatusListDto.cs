namespace ECommerce.Business.Models.Dtos.OrderStatuses
{
    public class OrderStatusListDto
    {
        public Guid Id { get; set; }
        public Entity.Enums.OrderStatus Status { get; set; }
    }
}
