using Stock.Domain.Entities;
using Stock.Domain.Entities.Views;

namespace Stock.Domain.Interfaces;

/// <summary>
/// Repository interface for direct data access to Box entities and database-level hierarchy views.
/// </summary>
public interface IBoxRepository
{
    /// <summary>
    /// Finds a basic Box entity by its unique identifier.
    /// </summary>
    /// <param name="boxId">The unique ID of the box.</param>
    /// <returns>The <see cref="Box"/> entity if found; otherwise, null.</returns>
    Task<Box?> FindAsync(int boxId);

    /// <summary>
    /// Checks if a box with the specified ID exists in the database.
    /// </summary>
    /// <param name="boxId">The ID to check.</param>
    /// <returns>True if the box exists; otherwise, false.</returns>
    Task<bool> ExistsAsync(int boxId);

    /// <summary>
    /// Checks for a name collision within the same hierarchy level.
    /// </summary>
    /// <remarks>
    /// Used for validation to ensure no two boxes share the same name under the same parent container.
    /// </remarks>
    /// <param name="name">The name to check.</param>
    /// <param name="parentBoxId">The ID of the parent container.</param>
    /// <param name="excludeId">An optional ID to exclude (useful during updates).</param>
    /// <returns>True if a duplicate name is found; otherwise, false.</returns>
    Task<bool> ExistsAsync(string name, int? parentBoxId, int? excludeId = null);

    /// <summary>
    /// Retrieves a detailed view of a box, typically joining metadata from related entities.
    /// </summary>
    /// <param name="boxId">The ID of the box.</param>
    /// <returns>A <see cref="BoxDetailed"/> view object if found; otherwise, null.</returns>
    Task<BoxDetailed?> GetBoxByIdAsync(int boxId);

    /// <summary>
    /// Retrieves a collection of boxes located directly under a specific parent.
    /// </summary>
    /// <param name="parentBoxId">The parent container ID. If null, searches the root level.</param>
    /// <returns>A list of <see cref="BoxList"/> view objects.</returns>
    Task<IEnumerable<BoxList>> GetBoxesByParentAsync(int? parentBoxId);

    /// <summary>
    /// Retrieves a simplified list of all boxes for selection/lookup purposes.
    /// </summary>
    /// <returns>A collection of <see cref="BoxLookupList"/> objects.</returns>
    Task<IEnumerable<BoxLookupList>> GetBoxesLookupAsync();

    /// <summary>
    /// Persists a new Box entity to the database.
    /// </summary>
    /// <param name="box">The box entity to add.</param>
    /// <returns>The ID assigned to the new box.</returns>
    Task<int> AddAsync(Box box);

    /// <summary>
    /// Updates an existing Box entity in the database.
    /// </summary>
    /// <param name="box">The box entity with updated values.</param>
    /// <returns>True if the database update was successful; otherwise, false.</returns>
    Task<bool> UpdateAsync(Box box);

    /// <summary>
    /// Removes a Box entity from the database.
    /// </summary>
    /// <param name="box">The box entity to remove.</param>
    /// <returns>True if the deletion was successful; otherwise, false.</returns>
    Task<bool> DeleteAsync(Box box);

    /// <summary>
    /// Directly updates the 'UpdatedAt' timestamp for a specific box record.
    /// </summary>
    /// <param name="boxId">The ID of the box to update.</param>
    /// <returns>The new <see cref="DateTime"/> value persisted to the database.</returns>
    Task<DateTime> ChangeUpdatedAtAsync(int boxId);

    /// <summary>
    /// Resolves the human-readable full path of a box based on its parent hierarchy.
    /// </summary>
    /// <remarks>
    /// Useful for displaying breadcrumbs or absolute location paths (e.g., "Main Warehouse > Section A > Box 1").
    /// </remarks>
    /// <param name="parentBoxId">The ID of the parent box to start the path resolution from.</param>
    /// <returns>The full path string if resolvable; otherwise, null.</returns>
    Task<string?> GetBoxFullPathByParentAsync(int? parentBoxId);
}