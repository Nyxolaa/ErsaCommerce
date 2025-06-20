using ErsaCommerce.Domain.Enum;

namespace ErsaCommerce.Application
{
    public class GetOrderDetailDto
    {
        public long Id { get; set; }
        public long CustomerId { get; set; } // musteri id'si
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }

        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending; // varsayilan, Pending
        public int ItemCount { get; set; }
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();

        public DateTime? ShippedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
    }
}
