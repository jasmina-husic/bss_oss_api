using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.CQRS.Commands.Products;

/// <summary>
/// Command to update an existing product.  If the product is not
/// found the handler returns <c>null</c>.
/// </summary>
public class UpdateProductCommand : ICommand<Product>
{
    public Product Product { get; set; } = null!;
}

public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, Product>
{
    private readonly ProductCatalogDbContext _ctx;
    public UpdateProductCommandHandler(ProductCatalogDbContext ctx) => _ctx = ctx;

    public async Task<Product> Handle(UpdateProductCommand cmd, CancellationToken ct)
    {
        var existing = await _ctx.Products.FindAsync(new object[]{cmd.Product.Id}, ct);
        if (existing is null) return null!;
        _ctx.Entry(existing).CurrentValues.SetValues(cmd.Product);
        await _ctx.SaveChangesAsync(ct);
        return existing;
    }
}