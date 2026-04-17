using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Domain.Entities.Views;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Persistence;

namespace Stock.Infrastructure.Repositories;

public class GlobalizationRepository(StockDbContext context) : IGlobalizationRepository
{
    public async Task<IEnumerable<Language>> GetAllLanguagesAsync() =>
        await context.Languages.AsNoTracking().ToListAsync();

    public async Task<IEnumerable<TranslationList>> GetAllTranslationsAsync()
    {
        return await context.Database
               .SqlQuery<TranslationList>($"EXEC [dbo].[GetAllTranslations]")
               .ToListAsync();
    }
}