using System;
using System.Collections.Generic;

namespace ProductCatalog.Domain.Entities
{
    /// <summary>
    /// Enterprise‑grade base class for all catalog specifications, adding versioning, validity period,
    /// lifecycle state and ownership metadata. Aligns with TMF SID <Specification> and <EntitySpecification>.
    /// </summary>
    public abstract class SpecificationBase : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // ---- Versioning ----
        public string Version { get; set; } = "1.0";
        public DateTime ValidFor_From { get; set; } = DateTime.UtcNow;
        public DateTime? ValidFor_To { get; set; }

        // ---- Status & Governance ----
        public LifecycleStatus LifecycleStatus { get; set; }
        public ICollection<RelatedParty> Owners { get; set; } = new List<RelatedParty>();
    }
}
