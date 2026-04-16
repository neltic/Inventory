namespace Stock.Domain.Entities;

/// <summary>
/// Represents a supported language within the system.
/// Acts as a catalog to ensure data integrity for translations.
/// </summary>
public class Language
{
    /// <summary>The unique identifier (e.g., "es-Mx", "en-Us").</summary>
    public string LanguageCode { get; set; } = null!;

    /// <summary>The friendly name of the language (e.g., "Español (México)").</summary>
    public string Name { get; set; } = null!;
        
    /// <summary>Indicates if this is the system's fallback language.</summary>
    public bool IsDefault { get; set; }

    /* --- Navigation Properties --- */
    public ICollection<Translation> Translations { get; set; } = [];
}