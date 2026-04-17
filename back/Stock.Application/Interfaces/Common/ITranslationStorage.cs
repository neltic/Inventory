namespace Stock.Application.Interfaces.Common;

public interface ITranslationStorage
{
    string DefaultLanguage { get; set; }

    void UpdateLanguage(string languageCode, IDictionary<string, IDictionary<string, string>> data);

    IDictionary<string, IDictionary<string, string>> GetLanguage(string languageCode);

    bool TryGetLanguageDictionary(string languageCode, out IDictionary<string, IDictionary<string, string>> data);

    bool IsValidLanguage(string languageCode);
}
