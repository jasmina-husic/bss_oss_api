namespace ProductCatalog.Domain.Entities
{
    public class ResourceSpecCharacteristicValue : BaseEntity
    {
        public string Value { get; set; } = string.Empty;
        public bool IsDefault { get; set; }

        public int ResourceSpecCharacteristicId { get; set; }
        public ResourceSpecCharacteristic ResourceSpecCharacteristic { get; set; } = null!;
    }
}
