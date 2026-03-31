using Stock.Application.DTOs;

namespace Stock.Application.Interfaces;

/// <summary>
/// Defines the contract for managing box entities.
/// </summary>
public interface IBoxService
{
    /// <summary>
    /// Retrieves full details of a specific box by its identifier.
    /// </summary>
    /// <param name="boxId">The unique identifier of the box.</param>
    /// <returns>A <see cref="BoxDetailedDto"/> if found; otherwise, null.</returns>
    Task<BoxDetailedDto?> GetBoxByIdAsync(int boxId);

    /// <summary>
    /// Retrieves a collection of boxes located within a specific parent container.
    /// </summary>
    /// <param name="parentBoxId">The ID of the parent box. If null, returns root-level boxes.</param>
    /// <returns>A list of boxes belonging to the specified parent.</returns>
    Task<IEnumerable<BoxListDto>> GetBoxesByParentBoxIdAsync(int? parentBoxId);

    /// <summary>
    /// Retrieves a lightweight list of all boxes for selection/lookup purposes.
    /// </summary>
    /// <returns>A collection of simplified box data (ID and Name).</returns>
    Task<IEnumerable<BoxLookupListDto>> GetBoxesLookupAsync();

    /// <summary>
    /// Creates a new box record in the system.
    /// </summary>
    /// <param name="dto">The data transfer object containing the new box details.</param>
    /// <returns>The ID of the newly created box.</returns>
    /// <exception cref="InvalidOperationException">Thrown when business rules are violated (e.g., duplicate names in the same level).</exception>
    Task<int> CreateAsync(BoxDto dto);

    /// <summary>
    /// Updates the information of an existing box.
    /// </summary>
    /// <param name="boxId">The ID of the box to update.</param>
    /// <param name="dto">The updated data for the box.</param>
    /// <returns>True if the update was successful; otherwise, false.</returns>
    Task<bool> UpdateAsync(int boxId, BoxDto dto);

    /// <summary>
    /// Deletes a box from the system.
    /// </summary>
    /// <remarks>
    /// Implementation should verify if the box is empty before deletion to maintain integrity.
    /// </remarks>
    /// <param name="boxId">The ID of the box to remove.</param>
    /// <returns>True if the deletion was successful; otherwise, false.</returns>
    Task<bool> DeleteAsync(int boxId);

    /// <summary>
    /// Refreshes the last modification timestamp for a specific box.
    /// </summary>
    /// <remarks>
    /// Usually called after assigning images or modifying internal storage to keep the metadata current.
    /// </remarks>
    /// <param name="boxId">The ID of the box to update.</param>
    /// <returns>The updated <see cref="DateTime"/> value.</returns>
    Task<DateTime> ChangeUpdatedAtAsync(int boxId);

    /// <summary>
    /// Retrieves a new box template or the first available empty box under a parent.
    /// </summary>
    /// <param name="parentBoxId">The ID of the parent container to search within.</param>
    /// <returns>A <see cref="BoxDetailedDto"/> representing an empty/new box state.</returns>
    Task<BoxDetailedDto> GetEmptyBoxByParentBoxIdAsync(int? parentBoxId);
}