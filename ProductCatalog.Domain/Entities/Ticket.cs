namespace ProductCatalog.Domain.Entities;

public class Ticket : BaseEntity
{
    public string TicketingId { get; set; } = string.Empty;

    public string CustomerCrmId { get; set; } = string.Empty;

    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = "Open";
    public string Priority { get; set; } = "Low";
    public string Owner { get; set; } = string.Empty;
}