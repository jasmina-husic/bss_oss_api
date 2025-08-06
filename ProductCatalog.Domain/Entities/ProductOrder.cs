namespace ProductCatalog.Domain.Entities;

public class ProductOrder : BaseEntity
{
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public int ProductOfferingId { get; set; }
    public ProductOffering ProductOffering { get; set; } = null!;
    public string Stage { get; set; } = string.Empty;
    public string ContractNumber { get; set; } = string.Empty;
    public DateTime ContractStartDate { get; set; }
    public DateTime? ContractEndDate { get; set; }
}
