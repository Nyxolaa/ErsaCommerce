using ErsaCommerce.Domain.Enum;

namespace ErsaCommerce.Domain
{
    public class Order : BaseEntity
    {
        public long CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }

        public Customer Customer { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
