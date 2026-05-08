namespace Stock.Application.DTOs;

/// <summary>
/// Data Transfer Object used for creating or updating a box.
/// Contains the core editable properties of a storage container.
/// </summary>
/// <param name="BoxId">The unique ID of the box. Use 0 when creating a new record.</param>
/// <param name="ParentBoxId">The ID of the parent container. Set to null if the box is located at the root level.</param>
/// <param name="Name">The descriptive name of the box (e.g., "North Rack - Level 2").</param>
/// <param name="BrandId">The identifier of the manufacturer or brand associated with the box.</param>
/// <param name="CategoryId">The identifier of the category for classification.</param>
/// <param name="Height">The vertical dimension (Height) of the box.</param>
/// <param name="Width">The horizontal dimension (Width) of the box.</param>
/// <param name="Depth">The front-to-back dimension (Depth) of the box.</param>
/// <param name="Notes">Any additional information, remarks, or specific storage instructions.</param>
public record BoxDto(
    int BoxId,
    int? ParentBoxId,
    string Name,
    int BrandId,
    int CategoryId,
    decimal Height,
    decimal Width,
    decimal Depth,
    string Notes
);