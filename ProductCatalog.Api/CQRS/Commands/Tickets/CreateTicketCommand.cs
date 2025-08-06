using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.CQRS.Commands.Tickets;

public class CreateTicketCommand : ICommand<Ticket>
{
    public string CustomerCrmId { get; set; } = string.Empty;
    public string TicketingId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = "Open";
    public string Priority { get; set; } = "Low";
    public string Owner { get; set; } = string.Empty;
}

public class CreateTicketCommandHandler : ICommandHandler<CreateTicketCommand, Ticket>
{
    private readonly ProductCatalogDbContext _ctx;
    public CreateTicketCommandHandler(ProductCatalogDbContext ctx) => _ctx = ctx;

    public async Task<Ticket> Handle(CreateTicketCommand cmd, CancellationToken ct)
    {
        if (await _ctx.Tickets.AnyAsync(t => t.TicketingId == cmd.TicketingId, ct))
            throw new InvalidOperationException($"TicketingId '{cmd.TicketingId}' already exists.");

        var customer = await _ctx.Customers.FirstOrDefaultAsync(c => c.CrmId == cmd.CustomerCrmId, ct);
        if (customer is null)
            throw new InvalidOperationException($"Customer with CRM ID '{cmd.CustomerCrmId}' not found.");

        var ticket = new Ticket
        {
            TicketingId = cmd.TicketingId,
            CustomerCrmId = cmd.CustomerCrmId,
            CustomerId = customer.Id,
            Title = cmd.Title,
            Status = cmd.Status,
            Priority = cmd.Priority,
            Owner = cmd.Owner
        };

        _ctx.Tickets.Add(ticket);
        await _ctx.SaveChangesAsync(ct);
        return ticket;
    }
}