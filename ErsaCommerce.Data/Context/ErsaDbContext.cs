using ErsaCommerce.Domain;
using Microsoft.EntityFrameworkCore;

namespace ErsaCommerce.Data
{
    public class ErsaDbContext : DbContext, IErsaDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get ; set ; }
        public DbSet<OrderItem> OrderItems { get ; set ; }
        public DbSet<Product> Products { get ; set ; }
    }
}
