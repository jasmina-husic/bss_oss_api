using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.CQRS.Queries.ProductOfferings;

/// <summary>
/// Query to fetch a specific product offering by its identifier.
/// Returns <c>null</c> if none exists.
/// </summary>
public class GetProductOfferingByIdQuery : IQuery<ProductOffering?>
{
    public int Id { get; set; }
}

public class GetProductOfferingByIdQueryHandler : IQueryHandler<GetProductOfferingByIdQuery, ProductOffering?>
{
    private readonly ProductCatalogDbContext _ctx;
    public GetProductOfferingByIdQueryHandler(ProductCatalogDbContext ctx) => _ctx = ctx;
    public async Task<ProductOffering?> Handle(GetProductOfferingByIdQuery q, CancellationToken ct)
        => await _ctx.ProductOfferings
            .Include(o => o.Components)
            .Include(o => o.PricePlan)
            .FirstOrDefaultAsync(o => o.Id == q.Id, ct);
}