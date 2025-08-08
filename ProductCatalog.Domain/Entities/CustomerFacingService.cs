using Microsoft.EntityFrameworkCore;
namespace ProductCatalog.Domain.Entities;

/// <summary>
/// Represents a customer‑facing service (CFS) in the TM Forum
/// information model.  These are service definitions that are
/// consumed directly by products or exposed to customers.  This
/// entity corresponds to the objects loaded from the UI's
/// <c>cfs.json</c> seed file and provides fields aligned with that
/// dataset.
/// </summary>
public class CustomerFacingService : BaseEntity
{
    /// <summary>The display name of the service.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>The category grouping for the service.</summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>The current lifecycle state (e.g. active/inactive).</summary>
    public string LifecycleState { get; set; } = string.Empty;

    /// <summary>The version identifier for the service specification.</summary>
    public string Version { get; set; } = string.Empty;

    /// <summary>
    /// References to underlying service specification identifiers that
    /// realise this customer‑facing service.  Mapped as an integer
    /// array using the Npgsql provider.
    /// </summary>
    public List<int> ServiceSpecIds { get; set; } = new();

    /// <summary>
    /// Arbitrary characteristics describing configurable aspects of
    /// the service.  This collection is owned by this entity and
    /// stored in a separate table.  Characteristics typically
    /// include a name, value type and whether the field is
    /// configurable.
    /// </summary>
    public List<Characteristic> Characteristics { get; set; } = new();

    /// <summary>
    /// Steps that should be executed to activate the service.  Mapped
    /// to a PostgreSQL array.  Empty if no activation sequence is
    /// defined.
    /// </summary>
    public List<string> ActivationSequence { get; set; } = new();
}

/// <summary>
/// Defines a simple characteristic associated with a
/// CustomerFacingService.  Characteristics capture metadata such as
/// name, data type and configurability.  This type is marked as
/// owned so EF Core will store the collection in a separate table
/// linked to the owning CFS entity.
/// </summary>
[Owned]
public class Characteristic
{
    public string Name { get; set; } = string.Empty;
    public string ValueType { get; set; } = string.Empty;
    public bool Configurable { get; set; } = false;
}