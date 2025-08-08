using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.CQRS.Commands.Tickets;

/// <summary>
/// Command to create a new support ticket.  Tickets are identified
/// internally by their numeric Id (the CNIS ID) and externally by
/// the DS identifier (<see cref="DsId"/>).  The requester must be
/// specified either by providing a RequesterId or by specifying
/// CustomerCrmId which will be looked up to obtain the internal
/// customer identifier.
/// </summary>
public class CreateTicketCommand : ICommand<Ticket>
{
    /// <summary>
    /// Optional DS identifier to assign to the ticket.  If not
    /// provided, the handler will generate a unique DS identifier.
    /// </summary>
    public string DsId { get; set; } = string.Empty;

    /// <summary>
    /// Optional CRM identifier for backwards compatibility.  If
    /// provided, it will be stored on the ticket and used to lookup
    /// the customer when RequesterId is not specified.
    /// </summary>
    public string CustomerCrmId { get; set; } = string.Empty;

    /// <summary>
    /// Internal customer identifier for the requester.  Overrides
    /// CustomerCrmId when specified.
    /// </summary>
    public int? RequesterId { get; set; }
    
    /// <summary>
    /// Short subject describing the ticket.
    /// </summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of the issue.  Optional.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Ticket status.  Should be one of NEW, OPEN, PENDING, HOLD,
    /// SOLVED or CLOSED.
    /// </summary>
    public string Status { get; set; } = "NEW";

    /// <summary>
    /// Ticket priority.  Should be LOW, NORMAL, HIGH or URGENT.
    /// </summary>
    public string Priority { get; set; } = "LOW";

    /// <summary>
    /// Identifier (username or email) of the person submitting the
    /// ticket.  Mirrors the created_by field in DS.
    /// </summary>
    public string Submitter { get; set; } = string.Empty;

    /// <summary>
    /// Identifier (username or email) of the user assigned to handle
    /// the ticket.
    /// </summary>
    public string Assignee { get; set; } = string.Empty;
}

public class CreateTicketCommandHandler : ICommandHandler<CreateTicketCommand, Ticket>
{
    private readonly ProductCatalogDbContext _ctx;
    public CreateTicketCommandHandler(ProductCatalogDbContext ctx) => _ctx = ctx;

    public async Task<Ticket> Handle(CreateTicketCommand cmd, CancellationToken ct)
    {
        // Resolve the requester (customer) identifier
        int requesterId;
        if (cmd.RequesterId.HasValue && cmd.RequesterId.Value > 0)
        {
            requesterId = cmd.RequesterId.Value;
            // Ensure the customer exists
            var customer = await _ctx.Customers.FirstOrDefaultAsync(c => c.Id == requesterId, ct);
            if (customer is null)
                throw new InvalidOperationException($"Customer with Id '{requesterId}' not found.");
        }
        else if (!string.IsNullOrWhiteSpace(cmd.CustomerCrmId))
        {
            var customer = await _ctx.Customers.FirstOrDefaultAsync(c => c.CrmId == cmd.CustomerCrmId, ct);
            if (customer is null)
                throw new InvalidOperationException($"Customer with CRM ID '{cmd.CustomerCrmId}' not found.");
            requesterId = customer.Id;
        }
        else
        {
            throw new InvalidOperationException("Either RequesterId or CustomerCrmId must be provided.");
        }

        // Generate DS identifier if not provided
        var dsId = string.IsNullOrWhiteSpace(cmd.DsId)
            ? $"DS-{DateTime.UtcNow:yyyyMMddHHmmssfff}"
            : cmd.DsId;

        // Ensure DS identifier is unique
        if (await _ctx.Tickets.AnyAsync(t => t.DsId == dsId, ct))
            throw new InvalidOperationException($"DsId '{dsId}' already exists.");

        var ticket = new Ticket
        {
            DsId = dsId,
            CustomerCrmId = cmd.CustomerCrmId,
            RequesterId = requesterId,
            Subject = cmd.Subject,
            Description = cmd.Description,
            Status = cmd.Status,
            Priority = cmd.Priority,
            Submitter = cmd.Submitter,
            Assignee = cmd.Assignee
        };

        _ctx.Tickets.Add(ticket);
        await _ctx.SaveChangesAsync(ct);
        return ticket;
    }
}