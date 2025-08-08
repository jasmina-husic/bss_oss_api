using System.ComponentModel.DataAnnotations.Schema;

namespace ProductCatalog.Domain.Entities;

/// <summary>
/// Represents a comment attached to a support ticket.  Comments are
/// synchronised with downstream systems like the DS and Zendesk but
/// stored locally in CNIS.  Each comment has its own primary key
/// (the CNIS comment ID) via <see cref="BaseEntity.Id"/> and also
/// stores the identifier assigned by the DS system for cross‑reference.
/// </summary>
public class TicketComment : BaseEntity
{
    /// <summary>
    /// Foreign key reference to the parent ticket.  This links the
    /// comment back to the ticket it belongs to.
    /// </summary>
    public int TicketId { get; set; }

    /// <summary>
    /// Navigation property to the ticket.  EF will populate this
    /// reference when explicitly loaded; it is optional on insert.
    /// </summary>
    public Ticket? Ticket { get; set; }

    /// <summary>
    /// Identifier assigned by the DS system for this comment.  This
    /// allows correlation with records in Zendesk or other systems.
    /// Not used as the primary key within CNIS.
    /// </summary>
    public string DsId { get; set; } = string.Empty;

    /// <summary>
    /// Text of the comment.  Contains the body content entered by
    /// the user.  Required.
    /// </summary>
    public string Comment { get; set; } = string.Empty;

    /// <summary>
    /// Identifier (such as username or email) of the author of the
    /// comment.  Mirrors the created_by field in DS.  Required.
    /// </summary>
    public string Author { get; set; } = string.Empty;
}