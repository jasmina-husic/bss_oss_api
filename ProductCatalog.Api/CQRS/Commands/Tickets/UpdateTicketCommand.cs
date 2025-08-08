using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.CQRS.Commands.Tickets;

/// <summary>
/// Command to update an existing ticket.  The ticket may be
/// identified either by its numeric Id (CNIS Id) or by its DS
/// identifier.  Only non-null properties on the command will
/// overwrite existing values on the ticket.
/// </summary>
public class UpdateTicketCommand : ICommand<Ticket>
{
    public int? Id { get; set; }
    public string DsId { get; set; } = string.Empty;
    public int? RequesterId { get; set; }
    public string? Subject { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public string? Priority { get; set; }
    public string? Submitter { get; set; }
    public string? Assignee { get; set; }
}

public class UpdateTicketCommandHandler : ICommandHandler<UpdateTicketCommand, Ticket>
{
    private readonly ProductCatalogDbContext _ctx;
    public UpdateTicketCommandHandler(ProductCatalogDbContext ctx) => _ctx = ctx;

    public async Task<Ticket> Handle(UpdateTicketCommand cmd, CancellationToken ct)
    {
        // Locate existing ticket by Id or DsId
        Ticket? ticket = null;
        if (cmd.Id.HasValue && cmd.Id.Value > 0)
        {
            ticket = await _ctx.Tickets.FirstOrDefaultAsync(t => t.Id == cmd.Id.Value, ct);
        }
        if (ticket is null && !string.IsNullOrWhiteSpace(cmd.DsId))
        {
            ticket = await _ctx.Tickets.FirstOrDefaultAsync(t => t.DsId == cmd.DsId, ct);
        }
        if (ticket is null) return null!;

        // Update requester if provided
        if (cmd.RequesterId.HasValue)
        {
            var customer = await _ctx.Customers.FirstOrDefaultAsync(c => c.Id == cmd.RequesterId.Value, ct);
            if (customer is null)
                throw new InvalidOperationException($"Customer with Id '{cmd.RequesterId.Value}' not found.");
            ticket.RequesterId = cmd.RequesterId.Value;
        }

        if (cmd.Subject is not null) ticket.Subject = cmd.Subject;
        if (cmd.Description is not null) ticket.Description = cmd.Description;
        if (cmd.Status is not null) ticket.Status = cmd.Status;
        if (cmd.Priority is not null) ticket.Priority = cmd.Priority;
        if (cmd.Submitter is not null) ticket.Submitter = cmd.Submitter;
        if (cmd.Assignee is not null) ticket.Assignee = cmd.Assignee;

        ticket.LastModified = DateTime.UtcNow;
        await _ctx.SaveChangesAsync(ct);
        return ticket;
    }
}