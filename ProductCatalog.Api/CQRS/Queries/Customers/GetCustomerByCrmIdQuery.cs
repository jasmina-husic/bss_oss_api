using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.CQRS.Queries.Customers;

/// <summary>
/// Fetch a customer (and addresses) by external CRM Id.
/// </summary>
public class GetCustomerByCrmIdQuery : IQuery<Customer?>
{
    public string CrmId { get; set; } = string.Empty;
}

public class GetCustomerByCrmIdQueryHandler : IQueryHandler<GetCustomerByCrmIdQuery,Customer?>
{
    private readonly ProductCatalogDbContext _ctx;
    public GetCustomerByCrmIdQueryHandler(ProductCatalogDbContext ctx)=>_ctx=ctx;

    public async Task<Customer?> Handle(GetCustomerByCrmIdQuery q, CancellationToken ct)
        => await _ctx.Customers
                     .Include(c=>c.BillingAddress)
                     .Include(c=>c.ShippingAddress)
                     .Include(c=>c.Tickets)
                     .FirstOrDefaultAsync(c=>c.CrmId==q.CrmId, ct);
}