using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.CQRS.Queries.Customers;

public class GetAllCustomersQuery : IQuery<IEnumerable<Customer>>{}

public class GetAllCustomersQueryHandler : IQueryHandler<GetAllCustomersQuery, IEnumerable<Customer>>
{
    private readonly ProductCatalogDbContext _ctx;
    public GetAllCustomersQueryHandler(ProductCatalogDbContext ctx)=>_ctx=ctx;
    public async Task<IEnumerable<Customer>> Handle(GetAllCustomersQuery q, CancellationToken ct)
        => await _ctx.Customers.ToListAsync(ct);
}
