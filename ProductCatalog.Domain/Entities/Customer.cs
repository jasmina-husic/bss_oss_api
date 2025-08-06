namespace ProductCatalog.Domain.Entities;

public class Customer : BaseEntity
{
    public string CrmId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Industry { get; set; } = string.Empty;
    public decimal AnnualRevenue { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string Fax { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
    public Address BillingAddress { get; set; } = new();
    public Address ShippingAddress { get; set; } = new();
    public int NumberOfEmployees { get; set; }
    public string Rating { get; set; } = string.Empty;
    public string AccountManager { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;

    public ICollection<ProductOrder> Orders { get; set; } = new List<ProductOrder>();
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
