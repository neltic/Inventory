namespace Stock.Application.DTOs;

/// <summary>
/// Data Transfer Object representing a specific product and its quantity stored within a box.
/// Used for listing the contents of a selected container.
/// </summary>
/// <param name="ItemId">The unique identifier of the item.</param>
/// <param name="Name">The descriptive name of the item.</param>
/// <param name="BrandId">The identifier of the brand associated with this specific stock entry.</param>
/// <param name="CategoryId">The identifier for the category classification.</param>
/// <param name="Quantity">The total number of units of this item/brand combination currently in the box.</param>
/// <param name="UpdatedAt">The timestamp of the last time this storage record was modified (e.g., quantity change).</param>
public record ItemInBoxListDto(
    int ItemId,
    string Name,
    int BrandId,
    int CategoryId,
    int Quantity,
    DateTimeOffset UpdatedAt
);