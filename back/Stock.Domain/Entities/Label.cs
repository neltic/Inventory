namespace Stock.Domain.Entities;

/// <summary>
/// Represents a unique text key used for globalization throughout the application.
/// Groups translations by context to optimize organization and retrieval.
/// </summary>
public class Label
{
    /// <summary>The unique identifier for the label key.</summary>
    public int LabelId { get; set; }

    /// <summary>
    /// The functional area or module where this label belongs (e.g., "Box", "Item").
    /// Helps in filtering and generating scoped JSON files.
    /// </summary>
    public string Context { get; set; } = null!;

    /// <summary>
    /// The specific code or key name used in code (e.g., "Title", "Name").
    /// </summary>
    public string LabelKey { get; set; } = null!;

    /* --- Navigation Properties --- */

    /// <summary>
    /// Collection of translations for this label in different languages.
    /// </summary>
    public ICollection<Translation> Translations { get; set; } = [];
}
