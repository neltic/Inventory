using Stock.Application.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Entities.Views;

namespace Stock.Application.Mappings;

public static class StorageMappingExtensions
{
    public static IEnumerable<ItemInBoxListDto> ToDtoList(this IEnumerable<ItemInBoxList> models)
    {
        return models.Select(i => new ItemInBoxListDto(
            i.ItemId,
            i.Name,
            i.BrandId,
            i.CategoryId,
            i.Quantity,
            i.UpdatedAt,
            i.Notes
            ));
    }

    public static IEnumerable<ItemStorageListDto> ToDtoList(this IEnumerable<ItemStorageList> models)
    {
        return models.Select(i => new ItemStorageListDto(
            i.BoxId,
            i.Name,
            i.BrandId,
            i.UpdatedAt,
            i.Quantity,
            i.Expires,
            i.ExpiresOn,
            i.Notes
            ));
    }

    public static Storage ToEntity(this StorageDto dto)
    {
        return new Storage()
        {
            BoxId = dto.BoxId,
            ItemId = dto.ItemId,
            BrandId = dto.BrandId,
            Quantity = dto.Quantity,
            Expires = dto.Expires,
            ExpiresOn = dto.ExpiresOn,
            Notes = dto.Notes
        };
    }

    public static StorageDto ToDto(this Storage model)
    {
        return new StorageDto(
            model.BoxId,
            model.ItemId,
            model.BrandId,
            model.Quantity,
            model.Expires,
            model.ExpiresOn,
            model.Notes
        );
    }
}