namespace ProductCatalog.Domain.Entities
{
    /// <summary>
    /// Reference to an external file or URL attached to a specification (TMF SID: Attachment).
    /// </summary>
    public class Attachment : BaseEntity
    {
        public string Url { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string MimeType { get; set; } = "application/pdf";
    }
}
