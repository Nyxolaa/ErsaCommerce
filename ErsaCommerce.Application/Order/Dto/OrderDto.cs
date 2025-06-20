using ErsaCommerce.Domain.Enum;

namespace ErsaCommerce.Application
{
    public class OrderDto
    {
        public long Id { get; set; }
        public long CustomerId { get; set; } // musteri id'si
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string ShippingAddress { get; set; }
        //public string BillingAddress { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending; // varsayilan, Pending
        public int ItemCount { get; set; }
        public List<CreateOrderItemDto> Items { get; set; } = new List<CreateOrderItemDto>();

        public DateTime? ShippedDate { get; set; }
        public DateTime? DeliveredDate { get; set; }

    }
}
