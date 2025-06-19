namespace ErsaCommerce.Domain
{
    public class Customer : BaseEntity
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;

        public ICollection<Order> Orders { get; set; } = new List<Order>();

    }
}
