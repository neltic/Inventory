using Stock.Application.DTOs;
using Stock.Domain.Entities;
using Stock.Domain.Entities.Views;

namespace Stock.Application.Mappings;

public static class BoxMappingExtensions
{
    public static BoxDetailedDto ToDto(this BoxDetailed model)
    {
        return new BoxDetailedDto(
            model.BoxId,
            model.ParentBoxId,
            model.Name,
            model.BrandId,
            model.CategoryId,
            model.Height,
            model.Width,
            model.Depth,
            model.Volume,
            model.Notes,
            model.CreatedAt,
            model.UpdatedAt,
            model.FullPath
        );
    }

    public static IEnumerable<BoxListDto> ToDtoList(this IEnumerable<BoxList> models)
    {
        return models.Select(b => new BoxListDto(
            b.BoxId,
            b.ParentBoxId,
            b.Name,
            b.CategoryId,
            b.BrandId,
            b.UpdatedAt,
            b.HasChildren,
            b.HasItems
        ));
    }

    public static IEnumerable<BoxLookupListDto> ToDtoList(this IEnumerable<BoxLookupList> models)
    {
        return models.Select(b => new BoxLookupListDto(
            b.BoxId,
            b.Name,
            b.UpdatedAt,
            b.Indent
        ));
    }

    public static IEnumerable<BoxTransferListDto> ToDtoList(this IEnumerable<BoxTransferList> models)
    {
        return models.Select(b => new BoxTransferListDto(
            b.BoxId,
            b.Name,
            b.UpdatedAt,
            b.Indent,
            b.IsSelectable
        ));
    }

    public static Box ToEntity(this BoxDto dto, int boxId)
    {
        return new Box
        {
            BoxId = boxId,
            ParentBoxId = dto.ParentBoxId,
            Name = dto.Name,
            BrandId = dto.BrandId,
            CategoryId = dto.CategoryId,
            Height = dto.Height,
            Width = dto.Width,
            Depth = dto.Depth,
            Notes = dto.Notes
        };
    }
}
