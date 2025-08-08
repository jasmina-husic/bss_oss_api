using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.CQRS.Commands.TicketComments;

/// <summary>
/// Command to add a new comment to an existing ticket.
/// </summary>
public class CreateTicketCommentCommand : ICommand<TicketComment>
{
    /// <summary>
    /// CNIS identifier of the ticket to which the comment belongs.
    /// Either TicketId or TicketDsId must be specified.
    /// </summary>
    public int? TicketId { get; set; }

    /// <summary>
    /// DS identifier of the ticket.  Used if TicketId is not
    /// provided.
    /// </summary>
    public string? TicketDsId { get; set; }

    /// <summary>
    /// Optional DS identifier to assign to the comment.
    /// </summary>
    public string DsId { get; set; } = string.Empty;

    /// <summary>
    /// Comment text.
    /// </summary>
    public string Comment { get; set; } = string.Empty;

    /// <summary>
    /// Identifier (username or email) of the author of the comment.
    /// </summary>
    public string Author { get; set; } = string.Empty;
}

public class CreateTicketCommentCommandHandler : ICommandHandler<CreateTicketCommentCommand, TicketComment>
{
    private readonly ProductCatalogDbContext _ctx;
    public CreateTicketCommentCommandHandler(ProductCatalogDbContext ctx) => _ctx = ctx;

    public async Task<TicketComment> Handle(CreateTicketCommentCommand cmd, CancellationToken ct)
    {
        // Find the ticket using either Id or DsId
        Ticket? ticket = null;
        if (cmd.TicketId.HasValue && cmd.TicketId.Value > 0)
        {
            ticket = await _ctx.Tickets.FirstOrDefaultAsync(t => t.Id == cmd.TicketId.Value, ct);
        }
        if (ticket is null && !string.IsNullOrWhiteSpace(cmd.TicketDsId))
        {
            ticket = await _ctx.Tickets.FirstOrDefaultAsync(t => t.DsId == cmd.TicketDsId, ct);
        }
        if (ticket is null)
        {
            throw new InvalidOperationException("Ticket not found.");
        }

        // Generate DS id for comment if not provided
        var dsId = string.IsNullOrWhiteSpace(cmd.DsId)
            ? $"DSC-{DateTime.UtcNow:yyyyMMddHHmmssfff}"
            : cmd.DsId;

        // Ensure DS comment id uniqueness
        if (await _ctx.TicketComments.AnyAsync(tc => tc.DsId == dsId, ct))
            throw new InvalidOperationException($"DsId '{dsId}' already exists for a ticket comment.");

        var comment = new TicketComment
        {
            TicketId = ticket.Id,
            DsId = dsId,
            Comment = cmd.Comment,
            Author = cmd.Author
        };

        _ctx.TicketComments.Add(comment);
        await _ctx.SaveChangesAsync(ct);
        return comment;
    }
}