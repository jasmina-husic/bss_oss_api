namespace ProductCatalog.Domain.Entities
{
    /// <summary>
    /// Hierarchical category used to classify services (TMF SID: ServiceCategory).
    /// </summary>
    public class ServiceCategory : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Self‑referencing hierarchy (optional)
        public int? ParentId { get; set; }
        public ServiceCategory? Parent { get; set; }
    }
}
