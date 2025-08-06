namespace ProductCatalog.Domain.Entities
{
    /// <summary>
    /// Defines QoS objectives for a service (SID: ServiceLevel).
    /// </summary>
    public class ServiceLevelSpecification : BaseEntity
    {
        public string Name { get; set; } = string.Empty; // e.g. "Gold"
        public string Description { get; set; } = string.Empty;
        public string ObjectiveTarget { get; set; } = string.Empty;   // e.g. 99.99
        public string ConformancePeriod { get; set; } = "Monthly";
    }
}
