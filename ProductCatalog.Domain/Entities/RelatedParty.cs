namespace ProductCatalog.Domain.Entities
{
    /// <summary>
    /// A party (person, organisation or system) that has a role with respect to a specification.
    /// Maps to TMF629 <RelatedParty>.
    /// </summary>
    public class RelatedParty : BaseEntity
    {
        public string Name { get; set; } = string.Empty;  // e.g. "Network Engineering"
        public string Role { get; set; } = string.Empty;  // e.g. "Owner", "SME"
    }
}
