namespace Stock.Application.DTOs;

/// <summary>
/// Data Transfer Object used to create or update a specific storage record.
/// Represents the unique link between a location (Box), a product (Item), and a Brand.
/// </summary>
/// <param name="BoxId">The unique identifier of the target storage box.</param>
/// <param name="ItemId">The unique identifier of the item being stored.</param>
/// <param name="BrandId">The unique identifier of the brand for this specific stock entry.</param>
/// <param name="Quantity">The number of units to be recorded in this location. Must be zero or greater.</param>
/// <param name="Expires">A flag indicating whether this specific batch of items has an expiration date.</param>
/// <param name="ExpiresOn">The specific expiration date (yyyy-MM-dd). Should be null if 'Expires' is false.</param>
public record StorageDto(
    int BoxId,
    int ItemId,
    int BrandId,
    int Quantity,
    bool Expires,
    DateOnly? ExpiresOn
);