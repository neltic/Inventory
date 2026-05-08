namespace Stock.Domain.Entities.Views;

public record BoxList(
    int BoxId,
    int? ParentBoxId,
    string Name,
    int CategoryId,
    int BrandId,
    DateTimeOffset ImageAt,
    bool HasChildren,
    bool HasItems
);