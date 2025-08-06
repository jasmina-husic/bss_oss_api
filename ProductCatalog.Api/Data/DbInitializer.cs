using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.Entities;
using System.Text.Json;

namespace ProductCatalog.Api.Data;

/// <summary>
/// • Seeds Customers & Tickets once (if the DB is empty).  
/// • Always bumps the identity sequences to MAX(Id)+1 so runtime inserts
///   can never collide with any explicit IDs in the seed data or added later.  
///   This runs on every application start.
/// </summary>
public static class DbInitializer
{
    public static async Task Initialize(ProductCatalogDbContext ctx)
    {
        string seedPath = Path.Combine(AppContext.BaseDirectory, "Data", "Seed");

     
        if (!await ctx.Customers.AnyAsync())
        {
            async Task<List<T>> Load<T>(string file)
            {
                var full = Path.Combine(seedPath, file);
                return File.Exists(full)
                    ? JsonSerializer.Deserialize<List<T>>(await File.ReadAllTextAsync(full)) ?? new()
                    : new();
            }

            var customers = await Load<Customer>("customers.json");
            var tickets = await Load<Ticket>("tickets.json");

            if (customers.Any()) await ctx.Customers.AddRangeAsync(customers);
            if (tickets.Any()) await ctx.Tickets.AddRangeAsync(tickets);

            await ctx.SaveChangesAsync();
        }

 
        await BumpSequence(ctx, "Customers");
        await BumpSequence(ctx, "Tickets");
    }

    /// <summary>Sets the identity sequence for &lt;table&gt;.Id to MAX(Id)+1.</summary>
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
