using Stock.Domain.Entities.Common;

namespace Stock.Domain.Entities;

/// <summary>
/// Represents a physical storage container (Box) within the inventory system.
/// Supports hierarchical nesting (boxes within boxes) and tracks physical dimensions.
/// </summary>
public class Box : AuditableEntity
{
    /// <summary>Unique identifier for the box.</summary>
    public int BoxId { get; set; }

    /// <summary>
    /// Foreign key for the parent container. 
    /// If null, this box is a "Root" container.
    /// </summary>
    public int? ParentBoxId { get; set; }

    /// <summary>Descriptive name of the box. Required field.</summary>
    public string Name { get; set; } = null!;

    /// <summary>Foreign key to the <see cref="Brand"/> associated with this box.</summary>
    public int BrandId { get; set; }

    /// <summary>Foreign key to the <see cref="Category"/> classification of this box.</summary>
    public int CategoryId { get; set; }

    /// <summary>Vertical dimension of the box.</summary>
    public decimal Height { get; set; }

    /// <summary>Horizontal dimension of the box.</summary>
    public decimal Width { get; set; }

    /// <summary>Front-to-back dimension of the box.</summary>
    public decimal Depth { get; set; }

    /// <summary>
    /// Total storage capacity. 
    /// Set to 'private' to ensure it is calculated based on dimensions and not manually overridden.
    /// </summary>
    public decimal Volume { get; private set; }

    /// <summary>Additional remarks, storage instructions, or specific location details.</summary>
    public string Notes { get; set; } = null!;

    /* --- Navigation Properties --- */

    /// <summary>Navigation property for the parent container.</summary>
    public Box? ParentBox { get; set; }

    /// <summary>Collection of nested boxes contained within this box (Child hierarchy).</summary>
    public ICollection<Box> SubBoxes { get; set; } = [];

    /// <summary>Collection of storage records representing physical items currently inside this box.</summary>
    public ICollection<Storage> Storages { get; set; } = [];

    /// <summary>Navigation property for the assigned category.</summary>
    public Category Category { get; set; } = null!;

    /// <summary>Navigation property for the assigned brand.</summary>
    public Brand Brand { get; set; } = null!;
}