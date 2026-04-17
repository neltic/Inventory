using Stock.Application.Interfaces;
using Stock.Application.Interfaces.Common;
using Stock.Domain.Entities.Views;
using Stock.Domain.Interfaces;

namespace Stock.Application.Services;

public class GlobalizationService(
    IGlobalizationRepository repository,
    ICacheService cache,
    ITranslationStorage storage)
    : BaseCacheService("globalization", "translation"), IGlobalizationService
{    
    public async Task InitializeCacheAsync()
    {
        var languages = await repository.GetAllLanguagesAsync();
        var defaultLang = languages.FirstOrDefault(l => l.IsDefault);

        if (defaultLang != null) storage.DefaultLanguage = defaultLang.LanguageCode;

        var translations = await repository.GetAllTranslationsAsync();

        foreach (var lang in languages)
        {
            await RefreshLanguageCacheAsync(lang.LanguageCode, translations);
        }
    }

    public string GetTranslation(string contextKey, params object[] values)
    {
        int dotIndex = contextKey.IndexOf('.');
        if (dotIndex <= 0) return contextKey;
        string context = contextKey[..dotIndex];
        string key = contextKey[(dotIndex + 1)..];
        return GetTranslation(context, key, values);
    }

    public string GetTranslation(string context, string key, params object[] values)
        => GetTranslation(storage.DefaultLanguage, context, key, values);

    public string GetTranslation(string languageCode, string context, string key, params object[] values)
    {
        if (storage.TryGetLanguage(languageCode, out var contexts) &&
            contexts.TryGetValue(context, out var labels) &&
            labels.TryGetValue(key, out var template))
        {
            return Interpolate(template, values);
        }

        if (languageCode != storage.DefaultLanguage &&
            storage.TryGetLanguage(storage.DefaultLanguage, out var defaultContexts) &&
            defaultContexts.TryGetValue(context, out var defaultLabels) &&
            defaultLabels.TryGetValue(key, out var defaultTemplate))
        {
            return Interpolate(defaultTemplate, values);
        }

        return $"{context}.{key}";
    }

    public async Task<IDictionary<string, IDictionary<string, string>>> GetLanguageJsonAsync(string languageCode)
    {
        // RAM
        if (storage.TryGetLanguage(languageCode, out var data)) return data;

        // Redis
        var redisData = await cache.GetAsync<IDictionary<string, IDictionary<string, string>>>(GetCacheKeyItem(languageCode));

        if (redisData != null)
        {
            storage.UpdateLanguage(languageCode, redisData);
            return redisData;
        }

        // RAM default
        if (languageCode != storage.DefaultLanguage && storage.TryGetLanguage(storage.DefaultLanguage, out var defaultData)) return defaultData;

        // Empty
        return new Dictionary<string, IDictionary<string, string>>();
    }

    private async Task RefreshLanguageCacheAsync(string languageCode, IEnumerable<TranslationList> translations)
    {
        var structuredData = translations
            .Where(t => t.LanguageCode == languageCode)
            .GroupBy(t => t.Context)
            .ToDictionary(
                g => g.Key,
                g => (IDictionary<string, string>)g.ToDictionary(t => t.LabelKey, t => t.Text)
            );

        storage.UpdateLanguage(languageCode, structuredData);

        await cache.SetAsync(GetCacheKeyItem(languageCode), structuredData, TimeSpan.FromHours(24));
    }

    private static string Interpolate(string template, params object[] values)
    {
        if (values == null || values.Length == 0) return template;
        var result = template;
        foreach (var obj in values)
        {
            if (obj == null) continue;
            foreach (var prop in obj.GetType().GetProperties())
            {
                var placeholder = $"{{{prop.Name}}}";
                if (result.Contains(placeholder))
                    result = result.Replace(placeholder, prop.GetValue(obj)?.ToString() ?? string.Empty);
            }
        }
        return result;
    }

}