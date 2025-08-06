namespace ProductCatalog.Domain.Entities;

public class Product : BaseEntity
{
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public ICollection<ProductOfferingComponent> OfferingComponents { get; set; } = new List<ProductOfferingComponent>();
}
