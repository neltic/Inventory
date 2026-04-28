namespace Stock.Domain.Entities.Views;

using System.ComponentModel.DataAnnotations.Schema;

public record BoxDetailed(
    int BoxId,
    int? ParentBoxId,
    string Name,
    int BrandId,
    int CategoryId,
    [property: Column(TypeName = "decimal(5, 2)")] decimal Height,
    [property: Column(TypeName = "decimal(5, 2)")] decimal Width,
    [property: Column(TypeName = "decimal(5, 2)")] decimal Depth,
    [property: Column(TypeName = "decimal(15, 2)")] decimal Volume,
    string Notes,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    string? FullPath,
    bool CanBeDeleted
);