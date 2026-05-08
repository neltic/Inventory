namespace Stock.Application.DTOs;

/// <summary>
/// Data Transfer Object used for creating or updating an inventory item.
/// Contains the fundamental properties required to define a product.
/// </summary>
/// <param name="ItemId">The unique identifier of the item. Use 0 when creating a new record.</param>
/// <param name="Name">The name of the item (e.g., "USB-C Hub" or "Hammer").</param>
/// <param name="Notes">Detailed description, specifications, or internal comments about the item.</param>
/// <param name="CategoryId">The identifier of the category this item belongs to.</param>
public record ItemDto(
    int ItemId,
    string Name,
    string Notes,
    int CategoryId
);