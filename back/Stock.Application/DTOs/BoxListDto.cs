namespace Stock.Application.DTOs;

/// <summary>
/// Lightweight DTO used for displaying boxes in lists, grids, or navigation trees.
/// </summary>
/// <param name="BoxId">The unique identifier for the box.</param>
/// <param name="ParentBoxId">The ID of the parent container. Null if located at the root level.</param>
/// <param name="Name">The display name of the box.</param>
/// <param name="CategoryId">The identifier for the category classification.</param>
/// <param name="BrandId">The identifier for the associated brand.</param>
/// <param name="UpdatedAt">The timestamp of the last modification, useful for cache invalidation or sorting.</param>
/// <param name="HasChildren">Indicates if this box contains other sub-boxes (nested containers).</param>
/// <param name="HasItems">Indicates if there are physical products currently stored inside this box.</param>
public record BoxListDto(
    int BoxId,
    int? ParentBoxId,
    string Name,
    int CategoryId,
    int BrandId,
    DateTimeOffset UpdatedAt,
    bool HasChildren,
    bool HasItems
);