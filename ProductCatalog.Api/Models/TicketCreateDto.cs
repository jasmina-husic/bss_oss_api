namespace ProductCatalog.Api.Models;

/// <summary>
/// Data transfer object used to create a new ticket via the API.
/// Fields correspond to the DS/Zendesk model.  At minimum either
/// RequesterId or CustomerCrmId must be provided.
/// </summary>
public class TicketCreateDto
{
    /// <summary>
    /// Optional DS identifier to assign to the ticket.
    /// </summary>
    public string? DsId { get; set; }

    /// <summary>
    /// Optional CRM identifier used to locate the requester when
    /// RequesterId is not provided.
    /// </summary>
    public string? CustomerCrmId { get; set; }

    /// <summary>
    /// Internal identifier of the requester (customer/account).
    /// Required if CustomerCrmId is not provided.
    /// </summary>
    public int? RequesterId { get; set; }

    /// <summary>
    /// Internal identifier of the customer associated with the ticket.  This
    /// property is provided for UI compatibility.  If specified, it will
    /// override <see cref="RequesterId"/> when resolving the customer.
    /// </summary>
    public int? CustomerId { get; set; }

    /// <summary>
    /// Short description of the ticket.
    /// </summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Title of the ticket (UI term).  When provided, this value will be
    /// used to populate <see cref="Subject"/>.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Detailed description of the issue.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Ticket status; defaults to NEW.
    /// </summary>
    public string Status { get; set; } = "NEW";

    /// <summary>
    /// Ticket priority; defaults to LOW.
    /// </summary>
    public string Priority { get; set; } = "LOW";

    /// <summary>
    /// Identifier (username or email) of the submitter.
    /// </summary>
    public string Submitter { get; set; } = string.Empty;

    /// <summary>
    /// Identifier (username or email) of the assignee.
    /// </summary>
    public string Assignee { get; set; } = string.Empty;

    /// <summary>
    /// Owner of the ticket (UI term).  When provided, this value will be
    /// used to populate <see cref="Assignee"/>.
    /// </summary>
    public string? Owner { get; set; }
}