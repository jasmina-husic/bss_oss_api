using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;

namespace ProductCatalog.Api.CQRS.Commands.ProductOfferings;

/// <summary>
/// Soft‑deletes a product offering.  When deleted the
/// <see cref="ProductOffering.IsDeleted"/> flag is set and no
/// cascading removes occur.  Returns true if the offering was
/// located and marked deleted; otherwise false.
/// </summary>
public class DeleteProductOfferingCommand : ICommand<bool>
{
    public int Id { get; set; }
}

public class DeleteProductOfferingCommandHandler : ICommandHandler<DeleteProductOfferingCommand, bool>
{
    private readonly ProductCatalogDbContext _ctx;
    public DeleteProductOfferingCommandHandler(ProductCatalogDbContext ctx) => _ctx = ctx;
    public async Task<bool> Handle(DeleteProductOfferingCommand cmd, CancellationToken ct)
    {
        var off = await _ctx.ProductOfferings.FirstOrDefaultAsync(o => o.Id == cmd.Id, ct);
        if (off is null) return false;
        off.IsDeleted = true;
        off.LastModified = DateTime.UtcNow;
        await _ctx.SaveChangesAsync(ct);
        return true;
    }
}