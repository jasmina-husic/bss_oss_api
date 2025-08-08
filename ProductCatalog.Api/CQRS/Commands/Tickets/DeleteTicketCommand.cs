using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;

namespace ProductCatalog.Api.CQRS.Commands.Tickets;

public class DeleteTicketCommand : ICommand<bool>
{
    /// <summary>
    /// CNIS identifier for the ticket.  Optional.
    /// </summary>
    public int? Id { get; set; }

    /// <summary>
    /// DS identifier for the ticket.  Optional; used when Id is not
    /// provided.
    /// </summary>
    public string? DsId { get; set; }
}

public class DeleteTicketCommandHandler : ICommandHandler<DeleteTicketCommand, bool>
{
    private readonly ProductCatalogDbContext _ctx;
    public DeleteTicketCommandHandler(ProductCatalogDbContext ctx) => _ctx = ctx;

    public async Task<bool> Handle(DeleteTicketCommand cmd, CancellationToken ct)
    {
        var ticket = cmd.Id.HasValue
            ? await _ctx.Tickets.FirstOrDefaultAsync(t => t.Id == cmd.Id.Value, ct)
            : await _ctx.Tickets.FirstOrDefaultAsync(t => t.DsId == cmd.DsId, ct);

        if (ticket is null) return false;

        ticket.IsDeleted = true;
        ticket.LastModified = DateTime.UtcNow;
        await _ctx.SaveChangesAsync(ct);
        return true;
    }
}