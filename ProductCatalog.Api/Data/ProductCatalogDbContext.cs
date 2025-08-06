using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.Data;

public class ProductCatalogDbContext : DbContext
{
    public ProductCatalogDbContext(DbContextOptions<ProductCatalogDbContext> options) : base(options) { }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductOffering> ProductOfferings => Set<ProductOffering>();
    public DbSet<ProductOrder> ProductOrders => Set<ProductOrder>();
    public DbSet<ProductOfferingComponent> ProductOfferingComponents => Set<ProductOfferingComponent>();
    public DbSet<Ticket> Tickets => Set<Ticket>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Soft delete filters
        modelBuilder.Entity<Customer>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Product>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<ProductOffering>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<ProductOrder>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Ticket>().HasQueryFilter(e => !e.IsDeleted);

        // Owned types
        modelBuilder.Entity<Customer>().OwnsOne(c => c.BillingAddress);
        modelBuilder.Entity<Customer>().OwnsOne(c => c.ShippingAddress);
        modelBuilder.Entity<ProductOffering>().OwnsOne(o => o.PricePlan);

        // Composite key
        modelBuilder.Entity<ProductOfferingComponent>()
            .HasKey(k => new { k.ProductOfferingId, k.ProductId });

        modelBuilder.Entity<ProductOfferingComponent>()
            .HasOne(poc => poc.ProductOffering)
            .WithMany(po => po.Components)
            .HasForeignKey(poc => poc.ProductOfferingId);

        modelBuilder.Entity<ProductOfferingComponent>()
            .HasOne(poc => poc.Product)
            .WithMany(p => p.OfferingComponents)
            .HasForeignKey(poc => poc.ProductId);
    }

    public override int SaveChanges()
    {
        UpdateSoftDelete();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateSoftDelete();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateSoftDelete()
    {
        foreach(var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch(entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.LastModified = DateTime.UtcNow;
                    entry.Entity.IsDeleted = false;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModified = DateTime.UtcNow;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.LastModified = DateTime.UtcNow;
                    break;
            }
        }
    }
}
