using Stock.Domain.Entities.Common;

namespace Stock.Domain.Entities;

/// <summary>
/// Represents the localized content for a specific Label in a specific language.
/// </summary>
public class Translation : AuditableEntity
{
    /// <summary>The unique identifier for the translation record.</summary>
    public int TranslationId { get; set; }

    /// <summary>
    /// The language culture code (e.g., "en", "es-Mx", "fr-Fr").
    /// </summary>
    public string LanguageCode { get; set; } = null!;

    /// <summary>Foreign key to the parent <see cref="Label"/>.</summary>
    public int LabelId { get; set; }

    /// <summary>The localized text value.</summary>
    public string Text { get; set; } = null!;

    /* --- Navigation Properties --- */

    /// <summary>The language associated with this translation.</summary>
    public Language Language { get; set; } = null!;

    /// <summary>The label key associated with this translation.</summary>
    public Label Label { get; set; } = null!;
}
