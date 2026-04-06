namespace Stock.Application.DTOs;

/// <summary>
/// Lightweight DTO for displaying items in summary lists or search results.
/// Optimized for performance by including only essential status flags.
/// </summary>
/// <param name="ItemId">The unique identifier for the item.</param>
/// <param name="Name">The display name of the item.</param>
/// <param name="CategoryId">The identifier for the category classification.</param>
/// <param name="UpdatedAt">The timestamp of the last modification, useful for sorting or list refreshing.</param>
/// <param name="HasStock">
/// A boolean flag indicating if at least one unit of this item exists in any storage location. 
/// Used for quick filtering in the UI.
/// </param>
public record ItemListDto(
    int ItemId,
    string Name,
    int CategoryId,
    DateTimeOffset UpdatedAt,
    bool HasStock
);