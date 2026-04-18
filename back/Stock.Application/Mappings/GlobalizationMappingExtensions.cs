using Stock.Application.DTOs;
using Stock.Domain.Entities;

namespace Stock.Application.Mappings;

public static class GlobalizationMappingExtensions
{
    public static IEnumerable<LanguageDto> ToDtoList(this IEnumerable<Language> models)
    {
        return models.Select(l => new LanguageDto(
            l.LanguageCode,
            l.Name,
            l.IsDefault
        ));
    }
}
