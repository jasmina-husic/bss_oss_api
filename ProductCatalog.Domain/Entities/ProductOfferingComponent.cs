namespace ProductCatalog.Domain.Entities;

public class ProductOfferingComponent
{
    public int ProductOfferingId { get; set; }
    public ProductOffering ProductOffering { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public int Quantity { get; set; }
    public string Billing { get; set; } = "oneOff";
}
