using Stock.Application.Interfaces.Common;
using System.Collections.Concurrent;

namespace Stock.Infrastructure.Services;

public class TranslationStorage : ITranslationStorage
{
    private readonly ConcurrentDictionary<string, IDictionary<string, IDictionary<string, string>>> _cache = new();
    public string DefaultLanguage { get; set; } = "en";

    public void UpdateLanguage(string languageCode, IDictionary<string, IDictionary<string, string>> data)
        => _cache.AddOrUpdate(languageCode, data, (_, _) => data);

    public IDictionary<string, IDictionary<string, string>> GetLanguage(string languageCode)
    {
        return _cache.TryGetValue(languageCode, out var data) ? data : new Dictionary<string, IDictionary<string, string>>();
    }

    public bool TryGetLanguage(string languageCode, out IDictionary<string, IDictionary<string, string>> data)
    {
        if (_cache.TryGetValue(languageCode, out var d))
        {
            data = d;
            return true;
        }
        data = new Dictionary<string, IDictionary<string, string>>();
        return false;
    }
}
