using System.Collections.Generic;

namespace ProductCatalog.Domain.Entities
{
    /// <summary>
    /// Abstract base for physical or logical resource specs (SID: ResourceSpecification).
    /// </summary>
    public abstract class ResourceSpecification : SpecificationBase
    {
        public ICollection<ResourceSpecCharacteristic> Characteristics { get; set; } = new List<ResourceSpecCharacteristic>();
    }
}
