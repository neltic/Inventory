namespace Stock.Application.DTOs;

/// <summary>
/// Data Transfer Object representing a specific storage instance of an item.
/// Used to display which boxes contain a particular product, including quantities and expiration tracking.
/// </summary>
/// <param name="BoxId">The unique identifier of the box where the item is stored.</param>
/// <param name="Name">The name of the box (e.g., "Main Warehouse - A1").</param>
/// <param name="BrandId">The identifier of the brand associated with this specific stock entry.</param>
/// <param name="UpdatedAt">The timestamp of the last modification to this storage record.</param>
/// <param name="Quantity">The current number of units available in this specific box.</param>
/// <param name="Expires">A flag indicating if this specific batch of items is subject to an expiration date.</param>
/// <param name="ExpiresOn">The specific expiration date. Null if 'Expires' is false.</param>
public record ItemStorageListDto(
    int BoxId,
    string Name,
    int BrandId,
    DateTimeOffset UpdatedAt,
    int Quantity,
    bool Expires,
    DateOnly? ExpiresOn
);