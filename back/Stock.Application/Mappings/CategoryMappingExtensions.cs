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

    public static Category ToEntity(this CategoryDto dto, int categoryId, Category? category = null)
    {
        if (category == null)
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
        else
        {
            category.CategoryId = categoryId;
            category.Name = dto.Name;
            category.Icon = dto.Icon;
            category.Color = dto.Color;
            category.Order = dto.Order;
            category.IncludedIn = dto.IncludedIn;
            return category;
        }
    }

}
