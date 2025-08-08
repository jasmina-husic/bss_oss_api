using ProductCatalog.Api.Data;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.CQRS.Commands.ProductOfferings;

/// <summary>
/// Command to create a new product offering.  Nested components and
/// price plan information are persisted along with the offering.  A
/// new identifier is generated and returned with the resulting
/// entity.
/// </summary>
public class CreateProductOfferingCommand : ICommand<ProductOffering>
{
    public ProductOffering ProductOffering { get; set; } = null!;
}

public class CreateProductOfferingCommandHandler : ICommandHandler<CreateProductOfferingCommand, ProductOffering>
{
    private readonly ProductCatalogDbContext _ctx;
    public CreateProductOfferingCommandHandler(ProductCatalogDbContext ctx) => _ctx = ctx;
    public async Task<ProductOffering> Handle(CreateProductOfferingCommand cmd, CancellationToken ct)
    {
        if (cmd.ProductOffering.Id != 0)
            cmd.ProductOffering.Id = 0;
        // Ensure nested components reference no Id so EF will assign
        if (cmd.ProductOffering.Components != null)
        {
            foreach (var c in cmd.ProductOffering.Components)
            {
                c.ProductOfferingId = 0;
            }
        }
        _ctx.ProductOfferings.Add(cmd.ProductOffering);
        await _ctx.SaveChangesAsync(ct);
        return cmd.ProductOffering;
    }
}