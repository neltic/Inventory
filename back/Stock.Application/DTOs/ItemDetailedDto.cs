namespace Stock.Application.DTOs;

/// <summary>
/// Represents the full detailed information of an inventory item, 
/// including its current location and audit metadata.
/// </summary>
/// <param name="ItemId">The unique identifier for the item.</param>
/// <param name="Name">The descriptive name of the item (e.g., "Screwdriver Set").</param>
/// <param name="Notes">Detailed description, specifications, or special handling instructions.</param>
/// <param name="CategoryId">The identifier for the category classification.</param>
/// <param name="CreatedAt">The timestamp when the item record was first created.</param>
/// <param name="UpdatedAt">The timestamp of the last modification to the item record.</param>
public record ItemDetailedDto(
    int ItemId,
    string Name,
    string Notes,
    int CategoryId,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);