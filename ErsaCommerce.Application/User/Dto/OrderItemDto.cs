namespace ErsaCommerce.Application
{
    public class OrderItemDto
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
