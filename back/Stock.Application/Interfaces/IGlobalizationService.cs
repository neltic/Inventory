namespace Stock.Application.Interfaces;

public interface IGlobalizationService
{
    Task InitializeCacheAsync();
    string GetTranslation(string context, string key, params object[] values);
    string GetTranslation(string languageCode, string context, string key, params object[] values);
    Task<IDictionary<string, IDictionary<string, string>>> GetLanguageJsonAsync(string languageCode);
}