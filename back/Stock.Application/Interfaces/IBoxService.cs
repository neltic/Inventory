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
    /// Retrieves a hierarchical list of potential parent destinations for a box.
    /// </summary>
    /// <param name="targetBoxId">
    /// The ID of the box being moved. 
    /// <para>
    /// Pass <see langword="null"/> when creating a new box to get all possible 
    /// destinations with <c>IsSelectable = true</c>.
    /// </para>
    /// <para>
    /// Pass a valid ID when moving an existing box; the current parent and 
    /// descendants will be marked with <c>IsSelectable = false</c> to prevent 
    /// circular references or redundant moves.
    /// </para>
    /// </param>
    /// <returns>
    /// An enumerable of <see cref="BoxTransferDto"/> representing the 
    /// warehouse tree, including a virtual "[Root]" option at the top.
    /// </returns>
    Task<IEnumerable<BoxTransferListDto>> GetAvailableParentBoxesByAsync(int? targetBoxId);

    /// <summary>
    /// Updates the parent container of a specific box, effectively moving it within the warehouse hierarchy.
    /// </summary>
    /// <param name="boxId">The unique identifier of the box to be moved.</param>
    /// <param name="newParentId">
    /// The identifier of the destination parent box. 
    /// Use <see langword="null"/> to move the box to the system root level.
    /// </param>
    /// <returns>
    /// A task representing the asynchronous operation. 
    /// The task result contains <see langword="true"/> if the box was successfully moved; 
    /// otherwise, <see langword="false"/>.
    /// </returns>
    /// <remarks>
    /// This method invokes the <c>[dbo].[MoveBox]</c> stored procedure, which handles 
    /// transaction integrity, updated timestamps, and prevents circular references 
    /// (e.g., moving a box into one of its own descendants).
    /// </remarks>
    Task<bool> MoveBoxAsync(int boxId, int? newParentId);

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