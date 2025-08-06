using System.Collections.Generic;

namespace ProductCatalog.Domain.Entities
{
    /// <summary>
    /// Abstract definition of a managed service. Inherits versioning from SpecificationBase.
    /// </summary>
    public abstract class ServiceSpecification : SpecificationBase
    {
        // Characteristics & QoS
        public ICollection<ServiceSpecCharacteristic> Characteristics { get; set; } = new List<ServiceSpecCharacteristic>();
        public ICollection<ServiceLevelSpecification> ServiceLevelSpecifications { get; set; } = new List<ServiceLevelSpecification>();

        // Composition / decomposition relationships
        public ICollection<ServiceSpecificationRelationship> ServiceRelationships { get; set; } = new List<ServiceSpecificationRelationship>();

        // Realisation by resources
        public ICollection<ResourceSpecification> RealizingResources { get; set; } = new List<ResourceSpecification>();
    }

    /// <summary>
    /// Service visible to the customer (CFS in SID). Adds category & documentation.
    /// </summary>
    public class CustomerFacingServiceSpecification : ServiceSpecification
    {
        public int ServiceCategoryId { get; set; }
        public ServiceCategory Category { get; set; } = null!;

        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    }

    /// <summary>
    /// Service supporting other services, not directly ordered by customers (RFS).
    /// </summary>
    public class ResourceFacingServiceSpecification : ServiceSpecification
    {
    }
}
