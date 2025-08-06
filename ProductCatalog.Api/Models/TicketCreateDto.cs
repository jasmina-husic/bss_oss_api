namespace ProductCatalog.Api.Models;

public class TicketCreateDto
{
    public string CustomerCrmId { get; set; } = string.Empty;
    public string TicketingId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = "Open";
    public string Priority { get; set; } = "Low";
    public string Owner { get; set; } = string.Empty;
}