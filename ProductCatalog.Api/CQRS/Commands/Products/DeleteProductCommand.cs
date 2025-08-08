using ProductCatalog.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace ProductCatalog.Api.CQRS.Commands.Products;

/// <summary>
/// Command to soft‑delete a product.  Returns true if the product
/// existed and was marked as deleted; false otherwise.  Soft delete
/// sets the <see cref="Product.IsDeleted"/> flag rather than
/// removing the row.
/// </summary>
public class DeleteProductCommand : ICommand<bool>
{
    public int Id { get; set; }
}

public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, bool>
{
    private readonly ProductCatalogDbContext _ctx;
    public DeleteProductCommandHandler(ProductCatalogDbContext ctx) => _ctx = ctx;

    public async Task<bool> Handle(DeleteProductCommand cmd, CancellationToken ct)
    {
        var prod = await _ctx.Products.FirstOrDefaultAsync(p => p.Id == cmd.Id, ct);
        if (prod is null) return false;
        prod.IsDeleted = true;
        prod.LastModified = DateTime.UtcNow;
        await _ctx.SaveChangesAsync(ct);
        return true;
    }
}