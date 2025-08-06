using System.Collections.Generic;

namespace ProductCatalog.Domain.Entities
{
    /// <summary>
    /// Defines a configurable attribute of a service (SID: ServiceSpecCharacteristic).
    /// </summary>
    public class ServiceSpecCharacteristic : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ValueType { get; set; } = "string";   // string, number, boolean, ...

        // ---- Extended fields ----
        public bool IsConfigurable { get; set; } = true;      // Can customer/order set this?
        public string? ValidationRuleRegex { get; set; }      // Optional regex for UI / API validation.

        public ICollection<ServiceSpecCharacteristicValue> PossibleValues { get; set; } = new List<ServiceSpecCharacteristicValue>();
    }
}
