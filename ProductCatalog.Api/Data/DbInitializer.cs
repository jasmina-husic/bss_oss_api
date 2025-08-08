using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.Entities;
using System.Text.Json;

namespace ProductCatalog.Api.Data;

/// <summary>
/// Handles database seeding on application startup.  This class loads
/// entity data from JSON files located in Data/Seed and populates
/// the EF Core context if the corresponding tables are empty.  It
/// also bumps identity sequences to prevent collisions with any
/// explicitly seeded identifiers.
/// </summary>
public static class DbInitializer
{
    public static async Task Initialize(ProductCatalogDbContext ctx)
    {
        string seedPath = Path.Combine(AppContext.BaseDirectory, "Data", "Seed");

        // Generic loader for seed files.  Returns an empty list if the
        // file is missing or cannot be deserialised.
        async Task<List<T>> Load<T>(string file)
        {
            var full = Path.Combine(seedPath, file);
            return File.Exists(full)
                ? JsonSerializer.Deserialize<List<T>>(await File.ReadAllTextAsync(full)) ?? new()
                : new();
        }

        // Seed Customers and Tickets.  Only seed if there are no
        // existing customers (tickets depend on customers).  The seed
        // files are expected to contain Id fields that will be used as
        // primary keys.  TicketComments are seeded after tickets.
        if (!await ctx.Customers.AnyAsync())
        {
            var customers = await Load<Customer>("customers.json");
            var tickets   = await Load<Ticket>("tickets.json");
            var comments  = await Load<TicketComment>("ticket_comments.json");
            if (customers.Any()) await ctx.Customers.AddRangeAsync(customers);
            if (tickets.Any())   await ctx.Tickets.AddRangeAsync(tickets);
            if (comments.Any())  await ctx.TicketComments.AddRangeAsync(comments);
            await ctx.SaveChangesAsync();
        }

        // Seed Products
        if (!await ctx.Products.AnyAsync())
        {
            var products = await Load<Product>("products.json");
            if (products.Any())
            {
                await ctx.Products.AddRangeAsync(products);
                await ctx.SaveChangesAsync();
            }
        }

        // Seed Product Offerings and components
        if (!await ctx.ProductOfferings.AnyAsync())
        {
            var offerings = await Load<ProductOffering>("offerings.json");
            if (offerings.Any())
            {
                await ctx.ProductOfferings.AddRangeAsync(offerings);
                await ctx.SaveChangesAsync();
            }
        }

        // Seed Customer Facing Services
        if (!await ctx.CustomerFacingServices.AnyAsync())
        {
            var cfss = await Load<CustomerFacingService>("cfs.json");
            if (cfss.Any())
            {
                await ctx.CustomerFacingServices.AddRangeAsync(cfss);
                await ctx.SaveChangesAsync();
            }
        }

        // Bump identity sequences for all seeded tables.  This ensures
        // that new inserts use identifiers greater than any explicit
        // values found in the seed files.
        await BumpSequence(ctx, "Customers");
        await BumpSequence(ctx, "Tickets");
        await BumpSequence(ctx, "TicketComments");
        await BumpSequence(ctx, "Products");
        await BumpSequence(ctx, "ProductOfferings");
        await BumpSequence(ctx, "CustomerFacingServices");
    }

    /// <summary>
    /// Sets the sequence for the given table's Id column to MAX(Id)+1.
    /// This avoids primary key collisions when inserting new entities
    /// after seeding.  Uses PostgreSQL's setval() function.
    /// </summary>
    private static async Task BumpSequence(DbContext ctx, string table)
    {
        var sql = $@"
                SELECT setval(
                    pg_get_serial_sequence('""{table}""','Id'),
                    COALESCE((SELECT MAX(""Id"") FROM ""{table}""), 0) + 1,
                    false
                );";
        await ctx.Database.ExecuteSqlRawAsync(sql);
    }
}
