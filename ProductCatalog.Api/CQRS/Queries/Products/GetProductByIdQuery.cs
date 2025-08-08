using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.CQRS.Queries.Products;

/// <summary>
/// Query to fetch a single product by its identifier.  Returns
/// <c>null</c> if no product with the given Id exists.
/// </summary>
public class GetProductByIdQuery : IQuery<Product?>
{
    public int Id { get; set; }
}

public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, Product?>
{
    private readonly ProductCatalogDbContext _ctx;
    public GetProductByIdQueryHandler(ProductCatalogDbContext ctx) => _ctx = ctx;
    public async Task<Product?> Handle(GetProductByIdQuery q, CancellationToken ct)
        => await _ctx.Products.FirstOrDefaultAsync(p => p.Id == q.Id, ct);
}