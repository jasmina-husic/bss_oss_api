using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.CQRS.Queries.Customers;

public class GetCustomerByIdQuery : IQuery<Customer>
{
    public int Id { get; set; }
}

public class GetCustomerByIdQueryHandler : IQueryHandler<GetCustomerByIdQuery,Customer>
{
    private readonly ProductCatalogDbContext _ctx;
    public GetCustomerByIdQueryHandler(ProductCatalogDbContext ctx)=>_ctx=ctx;
    public async Task<Customer> Handle(GetCustomerByIdQuery q, CancellationToken ct)
        => await _ctx.Customers.FirstOrDefaultAsync(e=>e.Id==q.Id, ct);
}
