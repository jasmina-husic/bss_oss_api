using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductCatalog.Domain.Entities;

public abstract class BaseEntity
{
    [Key]                                                     
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]    
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastModified { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
}
