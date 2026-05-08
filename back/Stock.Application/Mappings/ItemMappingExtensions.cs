using Stock.Application.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Entities.Views;

namespace Stock.Application.Mappings;

public static class ItemMappingExtensions
{
    public static ItemDetailedDto ToDto(this ItemDetailed entity) =>
        new(entity.ItemId, entity.Name, entity.Notes, entity.CategoryId, entity.ImageAt);

    public static Item ToEntity(this ItemDto dto, int itemId, Item? item = null)
    {
        if (item == null)
        {
            return new Item
            {
                ItemId = itemId,
                Name = dto.Name,
                Notes = dto.Notes,
                CategoryId = dto.CategoryId,
                ImageAt = DateTimeOffset.UtcNow
            };
        }
        else
        {
            item.ItemId = itemId;
            item.Name = dto.Name;
            item.Notes = dto.Notes;
            item.CategoryId = dto.CategoryId;
            return item;
        }
    }

    public static IEnumerable<ItemListDto> ToDtoList(this IEnumerable<ItemList> models)
    {
        return models.Select(b => new ItemListDto(
            b.ItemId,
            b.Name,
            b.CategoryId,
            b.ImageAt,
            b.HasStock
            ));
    }

    public static IEnumerable<ItemLocationListDto> ToDtoList(this IEnumerable<ItemLocationList> models)
    {
        return models.Select(b => new ItemLocationListDto(
            b.BoxId,
            b.Name,
            b.BrandId,
            b.Quantity
            ));
    }
}