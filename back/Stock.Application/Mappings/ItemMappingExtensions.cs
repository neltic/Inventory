using Stock.Application.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Entities.Views;

namespace Stock.Application.Mappings;

public static class ItemMappingExtensions
{
    public static ItemDetailedDto ToDto(this ItemDetailed entity) =>
        new(entity.ItemId, entity.Name, entity.Notes, entity.CategoryId, entity.InBox, entity.CreatedAt, entity.UpdatedAt);

    public static Item ToEntity(this ItemDto dto, int itemId) => new()
    {
        ItemId = itemId,
        Name = dto.Name,
        Notes = dto.Notes,
        CategoryId = dto.CategoryId
    };

    public static IEnumerable<ItemListDto> ToDtoList(this IEnumerable<ItemList> models)
    {
        return models.Select(b => new ItemListDto(
            b.ItemId,
            b.Name,
            b.CategoryId,
            b.UpdatedAt,
            b.HasStock
            ));
    }
}