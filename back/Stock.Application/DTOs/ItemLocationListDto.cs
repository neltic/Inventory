namespace Stock.Application.DTOs;

/// <summary>
/// Represents a specific storage location and stock level for an item.
/// </summary>
/// <param name="BoxId">The unique identifier of the box where the item is stored.</param>
/// <param name="Name">The descriptive name of the box.</param>
/// <param name="BrandId">The unique identifier of the brand associated with the item in this location.</param>
/// <param name="Quantity">The current stock level or number of units available in this specific box.</param>
public record ItemLocationListDto(
    int BoxId,
    string Name,
    int BrandId,
    int Quantity
);