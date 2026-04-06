namespace Stock.Application.DTOs;

/// <summary>
/// Specialized DTO for box transfer operations. 
/// Includes support for Root destination (null ID) and selection constraints.
/// </summary>
/// <param name="BoxId">The unique identifier for the box selection.</param>
/// <param name="Name">The display name of the box.</param>
/// <param name="UpdatedAt">The timestamp of the last modification, used to ensure the list is current.</param>
/// <param name="Indent">The depth level of the box in the hierarchy. Used by the UI to apply padding or prefixes (e.g., "— Box").</param>
/// <param name="IsSelectable">
/// Indicates if the box is a valid destination. 
/// Defaults to <c>true</c>. Set to <c>false</c> if the box is the current parent or part of a restricted branch 
/// to prevent circular references during transfers.
/// </param>
public record BoxTransferListDto(
    int? BoxId,
    string Name,
    DateTimeOffset UpdatedAt,
    int Indent,
    bool IsSelectable
);