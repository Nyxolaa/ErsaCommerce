using ErsaCommerce.Domain.Enum;

namespace ErsaCommerce.Application
{
    public class GetOrderDto
    {
        public long Id { get; set; }
        public long CustomerId { get; set; } 
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }

        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public int ItemCount { get; set; }

    }
}
