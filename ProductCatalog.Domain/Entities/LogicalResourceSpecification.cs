using Microsoft.EntityFrameworkCore;

namespace ProductCatalog.Domain.Entities
{
    /// <summary>
    /// Software, licences or other logical asset (SID: LogicalResourceSpec).
    /// </summary>
    public class LogicalResourceSpecification : ResourceSpecification
    {
        public string Protocol { get; set; } = string.Empty;  // e.g. BGP, HTTPS
        public Capacity Capacity { get; set; } = new Capacity();
    }

    /// <summary>
    /// Value object capturing capacity + units.
    /// </summary>
    [Owned]
    public class Capacity
    {
        public decimal Amount { get; set; }
        public string Unit { get; set; } = string.Empty; // GB, Cores, Licences...
    }
}
