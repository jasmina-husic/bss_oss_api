namespace ProductCatalog.Domain.Entities
{
    /// <summary>
    /// Relationship between two service specifications (SID: ServiceSpecRelationship).
    /// </summary>
    public class ServiceSpecificationRelationship : BaseEntity
    {
        public int SourceSpecificationId { get; set; }
        public ServiceSpecification SourceSpecification { get; set; } = null!;
        public int TargetSpecificationId { get; set; }
        public ServiceSpecification TargetSpecification { get; set; } = null!;
        public string RelationshipType { get; set; } = string.Empty; // DependsOn, Excludes, Aggregates, etc.
    }
}
