using ErsaCommerce.Data;
using ErsaCommerce.Domain;
using Microsoft.EntityFrameworkCore;

namespace ErsaCommerce.Infrastructure
{
    public static class DbInitializer
    {
        public static async Task SeedDefaultProductsAsync(IErsaDbContext context)
        {
            if (!await context.Products.AnyAsync())
            {
                // db de veri zaten varsa
                var hasAnyProduct = await context.Products.AnyAsync();
                if (hasAnyProduct) return;

                var now = DateTime.UtcNow;

                var defaultProducts = new List<Product>
                {
                    new()
                    {
                        Name = "Product 1",
                        Price = 99.99m,
                        Stock = 100,
                        Unique = Guid.NewGuid(),
                        CreatedAt = now,
                        CreatedBy = 1,
                        IsActive = true
                    },
                    new() 
                    {
                        Name = "Product 2",
                        Price = 49.50m,
                        Stock = 50,
                        Unique = Guid.NewGuid(),
                        CreatedAt = now,
                        CreatedBy = 1,
                        IsActive = true
                    }
                };

                context.Products.AddRange(defaultProducts);
                await context.SaveChangesAsync();
            }
        }
    }

}
