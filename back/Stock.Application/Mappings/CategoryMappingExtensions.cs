using Stock.Application.DTOs;
using Stock.Domain.Entities;

namespace Stock.Application.Mappings;

public static class CategoryMappingExtensions
{
    public static CategoryDto ToDto(this Category model)
    {
        return new CategoryDto(
            model.CategoryId,
            model.Name,
            model.Icon,
            model.Color,
            model.Order,
            model.IncludedIn
        );
    }

    public static IEnumerable<CategoryDto> ToDtoList(this IEnumerable<Category> models)
    {
        return models.Select(b => new CategoryDto(
            b.CategoryId,
            b.Name,
            b.Icon,
            b.Color,
            b.Order,
            b.IncludedIn
        ));
    }

    public static Category ToEntity(this CategoryDto dto, int categoryId)
    {
        return new Category
        {
            CategoryId = categoryId,
            Name = dto.Name,
            Icon = dto.Icon,
            Color = dto.Color,
            Order = dto.Order,
            IncludedIn = dto.IncludedIn
        };
    }

}
