using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.CQRS.Commands.ProductOfferings;

/// <summary>
/// Command to update an existing product offering including its
/// nested components and price plan.  Components are replaced
/// wholesale with the supplied collection.  Returns <c>null</c> if
/// the offering does not exist.
/// </summary>
public class UpdateProductOfferingCommand : ICommand<ProductOffering>
{
    public ProductOffering ProductOffering { get; set; } = null!;
}

public class UpdateProductOfferingCommandHandler : ICommandHandler<UpdateProductOfferingCommand, ProductOffering>
{
    private readonly ProductCatalogDbContext _ctx;
    public UpdateProductOfferingCommandHandler(ProductCatalogDbContext ctx) => _ctx = ctx;
    public async Task<ProductOffering> Handle(UpdateProductOfferingCommand cmd, CancellationToken ct)
    {
        var existing = await _ctx.ProductOfferings
            .Include(o => o.Components)
            .Include(o => o.PricePlan)
            .FirstOrDefaultAsync(o => o.Id == cmd.ProductOffering.Id, ct);
        if (existing is null) return null!;
        // Update scalar properties on the root
        _ctx.Entry(existing).CurrentValues.SetValues(cmd.ProductOffering);
        // Update price plan
        if (cmd.ProductOffering.PricePlan != null)
        {
            _ctx.Entry(existing.PricePlan).CurrentValues.SetValues(cmd.ProductOffering.PricePlan);
        }
        // Replace components
        if (cmd.ProductOffering.Components != null)
        {
            // Remove any existing components not present in the incoming list
            existing.Components.Clear();
            foreach (var comp in cmd.ProductOffering.Components)
            {
                // ensure EF knows this is a new component if id/keys not set
                comp.ProductOfferingId = existing.Id;
                existing.Components.Add(comp);
            }
        }
        await _ctx.SaveChangesAsync(ct);
        return existing;
    }
}