using Stock.Application.DTOs;

namespace Stock.Application.Interfaces;

/// <summary>
/// Provides services for application internationalization and localization, 
/// including translation management, language selection, and caching.
/// </summary>
public interface IGlobalizationService
{
    /// <summary>
    /// Gets or sets the current language code (e.g., "en", "es-MX") used for translations.
    /// </summary>
    string CurrentLanguage { get; set; }

    /// <summary>
    /// Preloads translation data into memory to improve performance during lookups.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task InitializeCacheAsync();

    /// <summary>
    /// Retrieves a collection of all supported languages in the system.
    /// </summary>
    /// <returns>A task that returns a list of <see cref="LanguageDto"/> objects.</returns>
    Task<IEnumerable<LanguageDto>> GetAllLanguagesAsync();

    /// <summary>
    /// Translates a label based on the <see cref="CurrentLanguage"/>, context, and key.
    /// Supports string formatting via additional values.
    /// </summary>
    /// <param name="context">The functional area or group (e.g., "Box", "Error").</param>
    /// <param name="key">The specific identifier for the label (e.g., "NOT_FOUND").</param>
    /// <param name="values">Optional parameters to replace placeholders in the translated string.</param>
    /// <returns>The localized string or a fallback if not found.</returns>
    string Translate(string context, string key, params object[] values);

    /// <summary>
    /// Translates a label for a specific language code, context, and key.
    /// </summary>
    /// <param name="languageCode">The target language code (e.g., "en").</param>
    /// <param name="context">The functional area or group.</param>
    /// <param name="key">The specific identifier for the label.</param>
    /// <param name="values">Optional parameters to replace placeholders.</param>
    /// <returns>The localized string for the requested language.</returns>
    string Translate(string languageCode, string context, string key, params object[] values);

    /// <summary>
    /// Retrieves the full dictionary of translations for all languages.
    /// </summary>
    /// <returns>
    /// A task returning a nested dictionary where keys are LanguageCodes, 
    /// containing Context/Key pairs and their translations.
    /// </returns>
    Task<IDictionary<string, IDictionary<string, string>>> GetLanguageDictionaryAsync();

    /// <summary>
    /// Retrieves all translations for a specific language.
    /// </summary>
    /// <param name="languageCode">The language code to retrieve.</param>
    /// <returns>
    /// A task returning a dictionary of Context/Key pairs and their translations for the specified language.
    /// </returns>
    Task<IDictionary<string, IDictionary<string, string>>> GetLanguageDictionaryAsync(string languageCode);
}