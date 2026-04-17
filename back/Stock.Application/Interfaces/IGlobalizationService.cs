namespace Stock.Application.Interfaces;

public interface IGlobalizationService
{
    string CurrentLanguage { get; set; }

    Task InitializeCacheAsync();

    string Translate(string context, string key, params object[] values);

    string Translate(string languageCode, string context, string key, params object[] values);

    Task<IDictionary<string, IDictionary<string, string>>> GetLanguageDictionaryAsync();

    Task<IDictionary<string, IDictionary<string, string>>> GetLanguageDictionaryAsync(string languageCode);
}