using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.CQRS.Queries.CustomerFacingServices;

/// <summary>
/// Query to return all customer‑facing services (CFS).
/// </summary>
public class GetAllCustomerFacingServicesQuery : IQuery<IEnumerable<CustomerFacingService>> { }

public class GetAllCustomerFacingServicesQueryHandler : IQueryHandler<GetAllCustomerFacingServicesQuery, IEnumerable<CustomerFacingService>>
{
    private readonly ProductCatalogDbContext _ctx;
    public GetAllCustomerFacingServicesQueryHandler(ProductCatalogDbContext ctx) => _ctx = ctx;
    public async Task<IEnumerable<CustomerFacingService>> Handle(GetAllCustomerFacingServicesQuery q, CancellationToken ct)
        => await _ctx.CustomerFacingServices
            .Include(c => c.Characteristics)
            .ToListAsync(ct);
}