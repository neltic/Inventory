using Stock.Domain.Entities.Common;

namespace Stock.Domain.Entities;

/// <summary>
/// Represents the physical inventory record (Stock) of a specific item within a specific box.
/// Acts as a junction entity that links Locations, Products, and Brands.
/// </summary>
/// <remarks>
/// This entity tracks stock levels and expiration metadata. 
/// The combination of BoxId, ItemId, and BrandId should typically be unique 
/// to prevent duplicate stock entries for the same product variant in the same location.
/// </remarks>
public class Storage : AuditableEntity
{
    /// <summary>The unique identifier for this specific storage record.</summary>
    public int StorageId { get; set; }

    /// <summary>Foreign key to the <see cref="Box"/> where the item is stored.</summary>
    public int BoxId { get; set; }

    /// <summary>Foreign key to the <see cref="Item"/> being stored.</summary>
    public int ItemId { get; set; }

    /// <summary>Foreign key to the <see cref="Brand"/> of the item in this specific batch.</summary>
    public int BrandId { get; set; }

    /// <summary>The current stock level. Must be a non-negative integer.</summary>
    public int Quantity { get; set; }

    /// <summary>Indicates if this stock entry has an associated expiration date.</summary>
    public bool Expires { get; set; }

    /// <summary>
    /// The expiration date of the items in this storage record. 
    /// Null if 'Expires' is false. Uses <see cref="DateOnly"/> for database efficiency.
    /// </summary>
    public DateOnly? ExpiresOn { get; set; }

    /* --- Navigation Properties --- */

    /// <summary>Navigation property for the physical location (Box).</summary>
    public Box Box { get; set; } = null!;

    /// <summary>Navigation property for the product (Item) stored.</summary>
    public Item Item { get; set; } = null!;

    /// <summary>Navigation property for the manufacturer or brand of the stored item.</summary>
    public Brand Brand { get; set; } = null!;
}