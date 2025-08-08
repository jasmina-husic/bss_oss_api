using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.CQRS.Queries.Products;

/// <summary>
/// Query to fetch all products from the catalog.  Returns the
/// complete list of <see cref="Product"/> entities.  Soft
/// deletion is applied at the DbContext level via query filters.
/// </summary>
public class GetAllProductsQuery : IQuery<IEnumerable<Product>> { }

public class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, IEnumerable<Product>>
{
    private readonly ProductCatalogDbContext _ctx;
    public GetAllProductsQueryHandler(ProductCatalogDbContext ctx) => _ctx = ctx;
    public async Task<IEnumerable<Product>> Handle(GetAllProductsQuery q, CancellationToken ct)
        => await _ctx.Products.ToListAsync(ct);
}