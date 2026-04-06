using Stock.Domain.Entities.Common;

namespace Stock.Application.DTOs;

/// <summary>
/// Data Transfer Object representing a category for items or boxes, 
/// including visual metadata and custom display ordering.
/// </summary>
/// <param name="CategoryId">The unique identifier for the category. Use 0 for new record creation.</param>
/// <param name="Name">The display name of the category (e.g., "Electronics", "Tools").</param>
/// <param name="Icon">The string identifier for a UI icon (e.g., a Material Design icon name like 'home' or 'inventory').</param>
/// <param name="Color">The HEX color code (e.g., "#FF5733") used for category labels or icons in the UI.</param>
/// <param name="Order">The sequential index used to determine the display position in lists or menus.</param>
/// <param name="IncludedIn">
/// Defines whether this category is applicable to Boxes, Items, or both. 
/// Used for filtering categories based on the current context.
/// </param>
public record CategoryDto(
    int CategoryId,
    string Name,
    string Icon,
    string Color,
    int Order,
    EntityScope IncludedIn
);