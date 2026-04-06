using Stock.Domain.Entities.Common;

namespace Stock.Domain.Entities;

/// <summary>
/// Represents a classification category for both Items and Boxes.
/// Includes visual metadata for UI icons and custom display ordering.
/// </summary>
public class Category
{
    /// <summary>Unique identifier for the category.</summary>
    public int CategoryId { get; set; }

    /// <summary>The display name of the category (e.g., "Hand Tools", "Large Containers"). Required.</summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// String identifier for a UI icon. 
    /// Typically maps to Material Design Icons or FontAwesome class names.
    /// </summary>
    public string Icon { get; set; } = null!;

    /// <summary>HEX color code for UI labels or icon accents (e.g., "#4CAF50").</summary>
    public string Color { get; set; } = null!;

    /// <summary>
    /// Custom sort index. Lower values appear first in lists and menus.
    /// Defaults to 0.
    /// </summary>
    public int Order { get; set; } = 0;

    /// <summary>
    /// Bitwise flag that determines if this category is applicable to Items, Boxes, or both.
    /// Defaults to <see cref="EntityScope.None"/>.
    /// </summary>
    public EntityScope IncludedIn { get; set; } = EntityScope.None;

    /* --- Navigation Properties --- */

    /// <summary>Collection of all individual items assigned to this category.</summary>
    public ICollection<Item> Items { get; set; } = new List<Item>();

    /// <summary>Collection of all storage boxes assigned to this category.</summary>
    public ICollection<Box> Boxes { get; set; } = new List<Box>();
}