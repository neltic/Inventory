using Stock.Application.DTOs;
using Stock.Domain.Entities.Views;

namespace Stock.Application.Mappings;

public static class AuditMappingExtensions
{
    public static IEnumerable<AuditListDto> ToDtoList(this IEnumerable<AuditList> models)
    {
        return models.Select(a => new AuditListDto(
            a.EventId,
            a.By,
            a.At
        ));
    }
}