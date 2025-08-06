using System.Collections.Generic;

namespace ProductCatalog.Domain.Entities
{
    /// <summary>
    /// Attribute of a resource (SID: ResourceSpecCharacteristic).
    /// </summary>
    public class ResourceSpecCharacteristic : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string ValueType { get; set; } = "string";
        public ICollection<ResourceSpecCharacteristicValue> PossibleValues { get; set; } = new List<ResourceSpecCharacteristicValue>();
    }
}
