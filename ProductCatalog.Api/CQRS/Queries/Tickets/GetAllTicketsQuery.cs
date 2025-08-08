using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.CQRS.Queries.Tickets;

public class GetAllTicketsQuery : IQuery<IEnumerable<Ticket>>{}

public class GetAllTicketsQueryHandler : IQueryHandler<GetAllTicketsQuery, IEnumerable<Ticket>>
{
    private readonly ProductCatalogDbContext _ctx;
    public GetAllTicketsQueryHandler(ProductCatalogDbContext ctx)=>_ctx=ctx;
    public async Task<IEnumerable<Ticket>> Handle(GetAllTicketsQuery q, CancellationToken ct)
        => await _ctx.Tickets
                 .Include(t => t.Requester)
                 .Include(t => t.Comments)
                 .ToListAsync(ct);
}
