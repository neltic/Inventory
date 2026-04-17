using Stock.Domain.Entities;
using Stock.Domain.Entities.Views;

namespace Stock.Domain.Interfaces;

public interface IGlobalizationRepository
{
    Task<IEnumerable<Language>> GetAllLanguagesAsync();

    Task<IEnumerable<TranslationList>> GetAllTranslationsAsync();
}
