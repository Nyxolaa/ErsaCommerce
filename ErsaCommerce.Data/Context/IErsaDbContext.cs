using ErsaCommerce.Domain;
using Microsoft.EntityFrameworkCore;

namespace ErsaCommerce.Data
{
    public interface IErsaDbContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<Order> Orders{ get; set; }
        DbSet<OrderItem> OrderItems { get; set; }
        DbSet<Product> Products { get; set; }
    }
}
