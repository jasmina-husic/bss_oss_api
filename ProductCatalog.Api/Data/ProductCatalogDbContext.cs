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

    /// <summary>
    /// Comments attached to support tickets.  Each comment is stored
    /// in its own table and relates back to a ticket via
    /// <see cref="TicketComment.TicketId"/>.  Soft deletion applies.
    /// </summary>
    public DbSet<TicketComment> TicketComments => Set<TicketComment>();

    /// <summary>
    /// Customer‑facing services exposed in the catalog.  These map
    /// directly to the CFS objects from the UI seed data and allow
    /// CRUD operations via the API.
    /// </summary>
    public DbSet<CustomerFacingService> CustomerFacingServices => Set<CustomerFacingService>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Soft delete filters
        modelBuilder.Entity<Customer>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Product>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<ProductOffering>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<ProductOrder>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Ticket>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<TicketComment>().HasQueryFilter(e => !e.IsDeleted);

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

        // Array mappings for simple lists on Product, ProductOffering and CFS
        // Npgsql supports mapping List<T> to array columns.  Explicitly
        // set the column types to ensure the correct array types are used.
        modelBuilder.Entity<Product>()
            .Property(p => p.Sequence)
            .HasColumnType("text[]");
        modelBuilder.Entity<Product>()
            .Property(p => p.CfsIds)
            .HasColumnType("integer[]");
        modelBuilder.Entity<ProductOffering>()
            .Property(o => o.ActivationSequence)
            .HasColumnType("text[]");
        modelBuilder.Entity<CustomerFacingService>()
            .Property(c => c.ServiceSpecIds)
            .HasColumnType("integer[]");
        modelBuilder.Entity<CustomerFacingService>()
            .Property(c => c.ActivationSequence)
            .HasColumnType("text[]");

        // Owned entity configuration for CustomerFacingService
        // Each Characteristic is stored as a separate row owned by
        // CustomerFacingService.  ActivationSequence and ServiceSpecIds
        // are mapped to Postgres array types by Npgsql provider.
        modelBuilder.Entity<CustomerFacingService>()
            .OwnsMany(c => c.Characteristics);

        modelBuilder.Entity<CustomerFacingService>().HasQueryFilter(e => !e.IsDeleted);

        // TicketComment relationship: a ticket has many comments.  Use
        // the TicketId foreign key on TicketComment.  When a ticket is
        // deleted, its comments remain but are also marked as deleted
        // via the soft delete mechanism.
        modelBuilder.Entity<TicketComment>()
            .HasOne(tc => tc.Ticket)
            .WithMany(t => t.Comments)
            .HasForeignKey(tc => tc.TicketId);
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
