namespace ProductCatalog.Domain.Entities;

public class ProductOffering : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public ICollection<ProductOfferingComponent> Components { get; set; } = new List<ProductOfferingComponent>();
    public PricePlan PricePlan { get; set; } = new();
    public ICollection<ProductOrder> Orders { get; set; } = new List<ProductOrder>();
}
