namespace ProductCatalog.Domain.Entities;

public class Product : BaseEntity
{
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// One‑off price for the product.  This corresponds to the
    /// "oneOff" field in the UI price model.  Nullable to
    /// distinguish between absent and zero values.
    /// </summary>
    public decimal? PriceOneOff { get; set; }

    /// <summary>
    /// Monthly recurring price for the product.  This corresponds to
    /// the "monthly" field in the UI price model.  Nullable to
    /// distinguish between absent and zero values.
    /// </summary>
    public decimal? PriceMonthly { get; set; }

    /// <summary>
    /// List of realisation steps required to fulfil the product.
    /// Stored as a PostgreSQL text[] via the Npgsql provider.  In the
    /// UI this maps to the "sequence" array.  An empty list
    /// indicates no defined steps.
    /// </summary>
    public List<string> Sequence { get; set; } = new();

    /// <summary>
    /// Identifiers of associated customer‑facing services (CFS) that
    /// this product depends on.  Mapped to an integer array in
    /// PostgreSQL.  In the UI this maps to the "cfsIds" array.
    /// </summary>
    public List<int> CfsIds { get; set; } = new();

    /// <summary>
    /// Navigation to any product offering components that include this
    /// product.  This collection is ignored from JSON output by
    /// default due to the reference loop handler configured in
    /// Program.cs.
    /// </summary>
    public ICollection<ProductOfferingComponent> OfferingComponents { get; set; } = new List<ProductOfferingComponent>();
}
