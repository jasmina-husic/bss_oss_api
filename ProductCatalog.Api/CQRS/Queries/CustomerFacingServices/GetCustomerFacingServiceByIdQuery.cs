using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.CQRS.Queries.CustomerFacingServices;

/// <summary>
/// Query to fetch a single customer‑facing service by its Id.
/// Returns <c>null</c> when not found.
/// </summary>
public class GetCustomerFacingServiceByIdQuery : IQuery<CustomerFacingService?>
{
    public int Id { get; set; }
}

public class GetCustomerFacingServiceByIdQueryHandler : IQueryHandler<GetCustomerFacingServiceByIdQuery, CustomerFacingService?>
{
    private readonly ProductCatalogDbContext _ctx;
    public GetCustomerFacingServiceByIdQueryHandler(ProductCatalogDbContext ctx) => _ctx = ctx;
    public async Task<CustomerFacingService?> Handle(GetCustomerFacingServiceByIdQuery q, CancellationToken ct)
        => await _ctx.CustomerFacingServices
            .Include(c => c.Characteristics)
            .FirstOrDefaultAsync(c => c.Id == q.Id, ct);
}