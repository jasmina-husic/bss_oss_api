using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.CQRS.Queries.TicketComments;

/// <summary>
/// Query to retrieve all comments for a given ticket.  The ticket
/// may be identified by its CNIS Id or by its DS identifier.
/// </summary>
public class GetCommentsByTicketQuery : IQuery<IEnumerable<TicketComment>>
{
    public int? TicketId { get; set; }
    public string? TicketDsId { get; set; }

    public class GetCommentsByTicketQueryHandler
        : IQueryHandler<GetCommentsByTicketQuery, IEnumerable<TicketComment>>
    {
        private readonly ProductCatalogDbContext _ctx;
        public GetCommentsByTicketQueryHandler(ProductCatalogDbContext ctx) => _ctx = ctx;

        public async Task<IEnumerable<TicketComment>> Handle(GetCommentsByTicketQuery q, CancellationToken ct)
        {
            if (q.TicketId.HasValue && q.TicketId.Value > 0)
            {
                return await _ctx.TicketComments
                                 .Where(tc => tc.TicketId == q.TicketId.Value)
                                 .ToListAsync(ct);
            }
            else if (!string.IsNullOrWhiteSpace(q.TicketDsId))
            {
                var ticket = await _ctx.Tickets.FirstOrDefaultAsync(t => t.DsId == q.TicketDsId, ct);
                if (ticket is null) return Enumerable.Empty<TicketComment>();
                return await _ctx.TicketComments
                                 .Where(tc => tc.TicketId == ticket.Id)
                                 .ToListAsync(ct);
            }
            else
            {
                return Enumerable.Empty<TicketComment>();
            }
        }
    }
}