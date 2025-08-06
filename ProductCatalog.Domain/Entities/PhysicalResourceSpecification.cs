using Microsoft.EntityFrameworkCore;

namespace ProductCatalog.Domain.Entities
{
    /// <summary>
    /// Hardware or other tangible asset (SID: PhysicalResourceSpec).
    /// </summary>
    public class PhysicalResourceSpecification : ResourceSpecification
    {
        public string Vendor { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;

        public int PowerConsumptionWatts { get; set; }
        public PhysicalDimensions Dimensions { get; set; } = new PhysicalDimensions();
    }

    /// <summary>
    /// Value object capturing HW dimensions.
    /// </summary>
    [Owned]
    public class PhysicalDimensions
    {
        public decimal HeightCm { get; set; }
        public decimal WidthCm { get; set; }
        public decimal DepthCm { get; set; }
    }
}
