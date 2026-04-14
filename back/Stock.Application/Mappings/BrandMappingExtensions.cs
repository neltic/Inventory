using Stock.Application.DTOs;
using Stock.Domain.Entities;

namespace Stock.Application.Mappings;

public static class BrandMappingExtensions
{
    public static BrandDto ToDto(this Brand model)
    {
        return new BrandDto(
            model.BrandId,
            model.Name,
            model.Color,
            model.Background,
            model.IncludedIn
        );
    }

    public static IEnumerable<BrandDto> ToDtoList(this IEnumerable<Brand> models)
    {
        return models.Select(b => new BrandDto(
            b.BrandId,
            b.Name,
            b.Color,
            b.Background,
            b.IncludedIn
        ));
    }

    public static Brand ToEntity(this BrandDto dto, int BrandId)
    {
        return new Brand
        {
            BrandId = BrandId,
            Name = dto.Name,
            Color = dto.Color,
            Background = dto.Background,
            IncludedIn = dto.IncludedIn
        };
    }
}
