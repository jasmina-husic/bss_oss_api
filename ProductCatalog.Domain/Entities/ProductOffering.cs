namespace ProductCatalog.Domain.Entities;

public class ProductOffering : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public ICollection<ProductOfferingComponent> Components { get; set; } = new List<ProductOfferingComponent>();
    public PricePlan PricePlan { get; set; } = new();
    public ICollection<ProductOrder> Orders { get; set; } = new List<ProductOrder>();

    /// <summary>
    /// Activation steps for this product offering.  These are
    /// presented to users to indicate how the offering will be
    /// activated/installed.  Stored as a text[] in PostgreSQL via
    /// the Npgsql provider.  Matches the UI field
    /// "activationSequence".  An empty list means there are no
    /// defined steps.
    /// </summary>
    public List<string> ActivationSequence { get; set; } = new();
}
