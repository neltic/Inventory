using Stock.Application.DTOs;
using Stock.Application.Interfaces;
using Stock.Application.Interfaces.Common;
using Stock.Application.Mappings;
using Stock.Domain.Entities.Views;
using Stock.Domain.Interfaces;

namespace Stock.Application.Services;

public class GlobalizationService(
    IGlobalizationRepository repository,
    ICacheService cache,
    ITranslationStorage storage)
    : BaseCacheService("globalization", "translation"), IGlobalizationService
{
    /// <inheritdoc />
    public string CurrentLanguage { get; set; } = string.Empty;

    /// <inheritdoc />
    public async Task InitializeCacheAsync()
    {
        var languages = await repository.GetAllLanguagesAsync();
        var defaultLanguage = languages.FirstOrDefault(l => l.IsDefault);

        if (defaultLanguage != null) storage.DefaultLanguage = defaultLanguage.LanguageCode;

        var translations = await repository.GetAllTranslationsAsync();

        foreach (var lang in languages)
        {
            await RefreshLanguageCacheAsync(lang.LanguageCode, translations);
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<LanguageDto>> GetAllLanguagesAsync()
    {
        var result = await repository.GetAllLanguagesAsync();
        return result.ToDtoList();
    }

    /// <inheritdoc />
    public string Translate(string context, string key, params object[] values)
        => Translate(CurrentLanguage, context, key, values);

    /// <inheritdoc />
    public string Translate(string languageCode, string context, string key, params object[] values)
    {
        if (storage.TryGetLanguageDictionary(languageCode, out var contexts) &&
            contexts.TryGetValue(context, out var labels) &&
            labels.TryGetValue(key, out var template))
        {
            return Format(template, values);
        }

        if (languageCode != storage.DefaultLanguage &&
            storage.TryGetLanguageDictionary(storage.DefaultLanguage, out var defaultContexts) &&
            defaultContexts.TryGetValue(context, out var defaultLabels) &&
            defaultLabels.TryGetValue(key, out var defaultTemplate))
        {
            return Format(defaultTemplate, values);
        }

        return $"{context}.{key}";
    }

    /// <inheritdoc />
    public async Task<IDictionary<string, IDictionary<string, string>>> GetLanguageDictionaryAsync()
    {
        return await GetLanguageDictionaryAsync(CurrentLanguage);
    }

    /// <inheritdoc />
    public async Task<IDictionary<string, IDictionary<string, string>>> GetLanguageDictionaryAsync(string languageCode)
    {
        // RAM
        if (storage.TryGetLanguageDictionary(languageCode, out var data)) return data;

        // Redis
        var redisData = await cache.GetAsync<IDictionary<string, IDictionary<string, string>>>(GetCacheKeyItem(languageCode));

        if (redisData != null)
        {
            storage.UpdateLanguage(languageCode, redisData);
            return redisData;
        }

        // RAM default
        if (languageCode != storage.DefaultLanguage && storage.TryGetLanguageDictionary(storage.DefaultLanguage, out var defaultData)) return defaultData;

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

    private static string Format(string template, params object[] values)
    {
        if (string.IsNullOrEmpty(template) || values == null || values.Length == 0)
            return template;

        try
        {
            return string.Format(template, values);
        }
        catch
        {
            return template;
        }
    }

}