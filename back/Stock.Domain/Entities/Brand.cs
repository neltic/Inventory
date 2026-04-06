using Stock.Domain.Entities.Common;

namespace Stock.Domain.Entities;

/// <summary>
/// Represents a brand or manufacturer associated with items or storage containers.
/// Includes visual metadata for UI rendering and scoping rules.
/// </summary>
public class Brand
{
    /// <summary>Unique identifier for the brand.</summary>
    public int BrandId { get; set; }

    /// <summary>Official name of the brand (e.g., "Bosch", "Samsung"). Required.</summary>
    public string Name { get; set; } = null!;

    /// <summary>HEX color code for the foreground/text (e.g., "#FFFFFF"). Used for UI badges.</summary>
    public string Color { get; set; } = null!;

    /// <summary>HEX color code for the background (e.g., "#000000"). Used for UI badges.</summary>
    public string Background { get; set; } = null!;

    /// <summary>
    /// Bitwise flag that determines the visibility and applicability of this brand.
    /// Default is <see cref="EntityScope.None"/>.
    /// </summary>
    public EntityScope IncludedIn { get; set; } = EntityScope.None;

    /* --- Navigation Properties --- */

    /// <summary>Collection of all boxes manufactured or branded by this entity.</summary>
    public ICollection<Box> Boxes { get; set; } = new List<Box>();

    /// <summary>
    /// Collection of specific storage records tied to this brand. 
    /// This represents physical item instances of this brand inside boxes.
    /// </summary>
    public ICollection<Storage> Storages { get; set; } = [];
}