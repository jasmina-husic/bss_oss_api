namespace ProductCatalog.Domain.Entities
{
    /// <summary>
    /// Allowed value for a ServiceSpecCharacteristic.
    /// </summary>
    public class ServiceSpecCharacteristicValue : BaseEntity
    {
        public string Value { get; set; } = string.Empty;
        public bool IsDefault { get; set; }

        public int ServiceSpecCharacteristicId { get; set; }
        public ServiceSpecCharacteristic ServiceSpecCharacteristic { get; set; } = null!;
    }
}
