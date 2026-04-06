using Stock.Domain.Entities.Common;

namespace Stock.Domain.Entities;

/// <summary>
/// Represents a unique product or item type within the inventory catalog.
/// Tracks general product metadata and its global categorization.
/// </summary>
public class Item : AuditableEntity
{
    /// <summary>The unique identifier for the product type.</summary>
    public int ItemId { get; set; }

    /// <summary>The official name of the item (e.g., "M3 Stainless Steel Bolt"). Required.</summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Detailed description, technical specifications, or internal notes 
    /// regarding the item's properties.
    /// </summary>
    public string Notes { get; set; } = null!;

    /// <summary>Foreign key to the <see cref="Category"/> assigned to this item.</summary>
    public int CategoryId { get; set; }

    /* --- Navigation Properties --- */

    /// <summary>
    /// Collection of all physical storage instances for this item. 
    /// Tracks which boxes contain this item and in what quantities.
    /// </summary>
    public ICollection<Storage> Storages { get; set; } = [];

    /// <summary>Navigation property for the classification category.</summary>
    public Category Category { get; set; } = null!;
}