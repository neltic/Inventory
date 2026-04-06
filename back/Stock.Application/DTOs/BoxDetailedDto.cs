namespace Stock.Application.DTOs;

/// <summary>
/// Represents the full detailed information of a storage box, 
/// including its hierarchy, dimensions, and metadata.
/// </summary>
/// <param name="BoxId">The unique identifier for the box.</param>
/// <param name="ParentBoxId">The ID of the parent box. Null if it is a root-level container.</param>
/// <param name="Name">The descriptive name of the box (e.g., "Shelf A-1").</param>
/// <param name="BrandId">The identifier of the brand associated with the box manufacturer or type.</param>
/// <param name="CategoryId">The identifier of the category this box belongs to.</param>
/// <param name="Height">The vertical dimension of the box in centimeters.</param>
/// <param name="Width">The horizontal dimension of the box in centimeters.</param>
/// <param name="Depth">The front-to-back dimension of the box in centimeters.</param>
/// <param name="Volume">The calculated total volume capacity of the box (usually Height * Width * Depth).</param>
/// <param name="Notes">Additional remarks or descriptions regarding the box's state or contents.</param>
/// <param name="CreatedAt">The timestamp when the box record was first created.</param>
/// <param name="UpdatedAt">The timestamp of the last modification to the box record.</param>
/// <param name="FullPath">The breadcrumb-style location path (e.g., "Warehouse > Section A > Box 1").</param>
public record BoxDetailedDto(
    int BoxId,
    int? ParentBoxId,
    string Name,
    int BrandId,
    int CategoryId,
    decimal Height,
    decimal Width,
    decimal Depth,
    decimal Volume,
    string Notes,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    string? FullPath
);