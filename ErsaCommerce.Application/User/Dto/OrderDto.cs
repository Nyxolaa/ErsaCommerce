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
        public string BillingAddress { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending; // varsayılan: Pending
        public int ItemCount { get; set; }
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public string PaymentMethod { get; set; } = "CreditCard"; // varsayılan: CreditCard
        public string ShippingMethod { get; set; } = "Standard"; // varsayılan: Standard
        public string? Notes { get; set; } // isteğe bağlı notlar
        public DateTime? ShippedDate { get; set; } // isteğe bağlı gönderim tarihi
        public DateTime? DeliveredDate { get; set; } // isteğe bağlı teslimat tarihi

    }
}
