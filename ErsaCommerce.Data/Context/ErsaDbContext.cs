using ErsaCommerce.Domain;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System;

namespace ErsaCommerce.Data
{
    public class ErsaDbContext : DbContext, IErsaDbContext
    {
        public ErsaDbContext(DbContextOptions<ErsaDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ErsaDbContext).Assembly);
            // tum BaseEntity'den tureyenler icin filtre 
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(ErsaDbContext)
                        .GetMethod(nameof(SetSoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static)
                        ?.MakeGenericMethod(entityType.ClrType);

                    method?.Invoke(null, new object[] { modelBuilder });
                }
            }
            base.OnModelCreating(modelBuilder);
        }

        private static void SetSoftDeleteFilter<TEntity>(ModelBuilder builder) where TEntity : BaseEntity
        {
            builder.Entity<TEntity>().HasQueryFilter(e => e.DeletedBy == null);
        }

        public int SaveChanges(long createdBy)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Unique = entry.Entity.Unique != Guid.Empty ? entry.Entity.Unique : Guid.NewGuid();
                        entry.Entity.CreatedBy = createdBy;
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.IsActive = true;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedBy = createdBy;
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.DeletedBy = createdBy;
                        entry.Entity.DeletedAt = DateTime.UtcNow;
                        entry.Entity.IsActive = false;
                        break;
                }
            }
            return base.SaveChanges();
        }

        public Task<int> SaveChangesAsync(long createdBy, CancellationToken cancellationToken)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Unique = entry.Entity.Unique != Guid.Empty ? entry.Entity.Unique : Guid.NewGuid();
                        entry.Entity.CreatedBy = createdBy;
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.IsActive = true;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedBy = createdBy;
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.DeletedBy = createdBy;
                        entry.Entity.DeletedAt = DateTime.UtcNow;
                        entry.Entity.IsActive = false;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);

        }
    }
}
