using ErsaCommerce.Domain.Enum;

namespace ErsaCommerce.Application
{
    public class GetOrderDto
    {
        public long Id { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DateTime CreatedAt { get; set; }
        public long CustomerId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new();
    }
}
