namespace ProductCatalog.Domain.Entities;

public class Ticket : BaseEntity
{
    /// <summary>
    /// Identifier assigned by the downstream (DS) system.  Mirrors the
    /// DS "id" field.  This value is distinct from the primary key
    /// used in CNIS (see <see cref="BaseEntity.Id"/>).
    /// </summary>
    public string DsId { get; set; } = string.Empty;

    /// <summary>
    /// Internal identifier of the customer associated with this ticket.  This
    /// mirrors the <see cref="RequesterId"/> property but is provided to
    /// support user interface models which refer to the customer by
    /// "customerId".  When seeding or creating tickets via the API,
    /// this should be set to the same value as <see cref="RequesterId"/>.
    /// </summary>
    public int CustomerId { get; set; }

    /// <summary>
    /// Optional CRM identifier for backwards compatibility.  Not used
    /// by the DS model.
    /// </summary>
    public string CustomerCrmId { get; set; } = string.Empty;

    /// <summary>
    /// Internal customer identifier for the requester of this ticket.
    /// Mirrors the DS field requester.id and replaces the previous
    /// CustomerId field.  See <see cref="Requester"/> for the
    /// navigation property.
    /// </summary>
    public int RequesterId { get; set; }

    /// <summary>
    /// Navigation property to the customer (account) that requested
    /// this ticket.
    /// </summary>
    public Customer? Requester { get; set; }

    /// <summary>
    /// Subject of the ticket.  Matches the DS and Zendesk
    /// terminology (previously called Title).
    /// </summary>
    public string Subject { get; set; } = string.Empty;

    /// <summary>
    /// Title of the ticket.  Provided to align with UI models which refer
    /// to the short description of a ticket as "title" rather than
    /// "subject".  In most cases this should mirror the value of
    /// <see cref="Subject"/> and vice versa.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of the ticket.  Added to align with DS
    /// which includes a description field.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Current status of the ticket.  Must be one of the allowed
    /// Zendesk statuses: NEW, OPEN, PENDING, HOLD, SOLVED or CLOSED.
    /// </summary>
    public string Status { get; set; } = "NEW";

    /// <summary>
    /// Priority of the ticket.  Must be one of LOW, NORMAL, HIGH or
    /// URGENT to match Zendesk conventions.
    /// </summary>
    public string Priority { get; set; } = "LOW";

    /// <summary>
    /// User identifier or email for the submitter of the ticket.  In
    /// DS this comes from the created_by field.
    /// </summary>
    public string Submitter { get; set; } = string.Empty;

    /// <summary>
    /// User identifier or email for the assignee handling the ticket.
    /// Replaces the previous Owner field.
    /// </summary>
    public string Assignee { get; set; } = string.Empty;

    /// <summary>
    /// Owner of the ticket.  Provided to align with UI models which refer
    /// to the user assigned to the ticket as the "owner".  This value
    /// should generally mirror the <see cref="Assignee"/> property.
    /// </summary>
    public string Owner { get; set; } = string.Empty;

    /// <summary>
    /// Collection of comments associated with this ticket.  Comments
    /// are loaded lazily and persisted in a separate table via the
    /// DbContext.
    /// </summary>
    public ICollection<TicketComment> Comments { get; set; } = new List<TicketComment>();
}