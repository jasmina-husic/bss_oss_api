using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.CQRS.Commands.Tickets;

public class UpdateTicketCommand : ICommand<Ticket>
{
    public string TicketingId { get; set; } = string.Empty;   // alternative key
    public Ticket Ticket { get; set; } = null!;
}

public class UpdateTicketCommandHandler : ICommandHandler<UpdateTicketCommand, Ticket>
{
    private readonly ProductCatalogDbContext _ctx;
    public UpdateTicketCommandHandler(ProductCatalogDbContext ctx) => _ctx = ctx;

    public async Task<Ticket> Handle(UpdateTicketCommand cmd, CancellationToken ct)
    {
        Ticket? existing = null;

        if (cmd.Ticket.Id != 0)
            existing = await _ctx.Tickets.FirstOrDefaultAsync(t => t.Id == cmd.Ticket.Id, ct);

        if (existing is null && !string.IsNullOrWhiteSpace(cmd.TicketingId))
            existing = await _ctx.Tickets.FirstOrDefaultAsync(t => t.TicketingId == cmd.TicketingId, ct);

        if (existing is null) return null!;

        existing.Title = cmd.Ticket.Title;
        existing.Status = cmd.Ticket.Status;
        existing.Priority = cmd.Ticket.Priority;
        existing.Owner = cmd.Ticket.Owner;
        existing.LastModified = DateTime.UtcNow;


        await _ctx.SaveChangesAsync(ct);
        return existing;
    }
}