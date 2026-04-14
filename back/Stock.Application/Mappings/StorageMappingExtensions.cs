using Stock.Application.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Entities.Views;

namespace Stock.Application.Mappings;

public static class StorageMappingExtensions
{
    public static IEnumerable<ItemInBoxListDto> ToDtoList(this IEnumerable<ItemInBoxList> models)
    {
        return models.Select(b => new ItemInBoxListDto(
            b.ItemId,
            b.Name,
            b.BrandId,
            b.CategoryId,
            b.Quantity,
            b.UpdatedAt
            ));
    }

    public static IEnumerable<ItemStorageListDto> ToDtoList(this IEnumerable<ItemStorageList> models)
    {
        return models.Select(b => new ItemStorageListDto(
            b.BoxId,
            b.Name,
            b.BrandId,
            b.UpdatedAt,
            b.Quantity,
            b.Expires,
            b.ExpiresOn
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
            model.ExpiresOn
        );
    }
}