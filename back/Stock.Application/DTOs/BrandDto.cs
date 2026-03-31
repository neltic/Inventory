using Stock.Domain.Entities.Common;

namespace Stock.Application.DTOs;

/// <summary>
/// Data Transfer Object representing a Brand, including UI styling and application scope.
/// </summary>
/// <param name="BrandId">The unique identifier for the brand. Use 0 for new brand creation.</param>
/// <param name="Name">The official name of the brand or manufacturer.</param>
/// <param name="Color">The foreground HEX color code (e.g., "#FFFFFF") for UI text or icons.</param>
/// <param name="Background">The background HEX color code (e.g., "#000000") for UI badges or labels.</param>
/// <param name="IncludedIn">
/// Defines the visibility scope of the brand (e.g., only for Boxes, only for Items, or Global).
/// Helps filter the brand selection in different forms.
/// </param>
public record BrandDto(
    int BrandId,
    string Name,
    string Color,
    string Background,
    EntityScope IncludedIn
);