using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.CQRS.Queries.Tickets;

/// <summary>
/// Retrieve a ticket by internal Id **or** external DS identifier.
/// Supply exactly one of them.
/// </summary>
public class GetTicketByIdQuery : IQuery<Ticket?>
{
    public int Id { get; set; }
    /// <summary>
    /// DS identifier for the ticket.  Optional; used when Id is not
    /// specified.
    /// </summary>
    public string? DsId { get; set; }

    public class GetTicketByIdQueryHandler
            : IQueryHandler<GetTicketByIdQuery, Ticket?>
    {
        private readonly ProductCatalogDbContext _ctx;
        public GetTicketByIdQueryHandler(ProductCatalogDbContext ctx) => _ctx = ctx;

        public async Task<Ticket?> Handle(GetTicketByIdQuery q,
                                          CancellationToken ct)
        {
            if (!string.IsNullOrWhiteSpace(q.DsId))
            {
                return await _ctx.Tickets
                                 .Include(t => t.Requester)
                                 .Include(t => t.Comments)
                                 .FirstOrDefaultAsync(t => t.DsId == q.DsId, ct);
            }

            return await _ctx.Tickets
                             .Include(t => t.Requester)
                             .Include(t => t.Comments)
                             .FirstOrDefaultAsync(t => t.Id == q.Id, ct);
        }
    }
}
