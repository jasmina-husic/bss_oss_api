using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.CQRS.Queries.Tickets;

/// <summary>
/// Retrieve a ticket by internal Id **or** external TicketingId.
/// Supply exactly one of them.
/// </summary>
public class GetTicketByIdQuery : IQuery<Ticket?>
{
    public int Id { get; set; }
    public string? TicketingId { get; set; }

    public class GetTicketByIdQueryHandler
            : IQueryHandler<GetTicketByIdQuery, Ticket?>
    {
        private readonly ProductCatalogDbContext _ctx;
        public GetTicketByIdQueryHandler(ProductCatalogDbContext ctx) => _ctx = ctx;

        public async Task<Ticket?> Handle(GetTicketByIdQuery q,
                                          CancellationToken ct)
        {
            if (!string.IsNullOrWhiteSpace(q.TicketingId))
            {
                return await _ctx.Tickets
                                 .Include(t => t.Customer)
                                 .FirstOrDefaultAsync(t => t.TicketingId == q.TicketingId, ct);
            }

            return await _ctx.Tickets
                             .Include(t => t.Customer)
                             .FirstOrDefaultAsync(t => t.Id == q.Id, ct);
        }
    }
}
