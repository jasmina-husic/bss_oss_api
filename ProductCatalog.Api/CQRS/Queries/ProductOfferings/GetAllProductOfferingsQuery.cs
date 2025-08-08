using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.CQRS.Queries.ProductOfferings;

/// <summary>
/// Query to fetch all product offerings.  Returns the list of
/// <see cref="ProductOffering"/> entities including nested
/// components and price plans.
/// </summary>
public class GetAllProductOfferingsQuery : IQuery<IEnumerable<ProductOffering>> { }

public class GetAllProductOfferingsQueryHandler : IQueryHandler<GetAllProductOfferingsQuery, IEnumerable<ProductOffering>>
{
    private readonly ProductCatalogDbContext _ctx;
    public GetAllProductOfferingsQueryHandler(ProductCatalogDbContext ctx) => _ctx = ctx;
    public async Task<IEnumerable<ProductOffering>> Handle(GetAllProductOfferingsQuery q, CancellationToken ct)
        => await _ctx.ProductOfferings
            .Include(o => o.Components)
            .Include(o => o.PricePlan)
            .ToListAsync(ct);
}