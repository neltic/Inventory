using Stock.Domain.Entities;
using Stock.Domain.Entities.Views;

namespace Stock.Domain.Interfaces;

/// <summary>
/// Repository interface for direct data access to Item entities and database-level item views.
/// </summary>
public interface IItemRepository
{
    /// <summary>
    /// Finds a basic Item entity by its unique identifier.
    /// </summary>
    /// <param name="itemId">The unique ID of the item.</param>
    /// <returns>The <see cref="Item"/> entity if found; otherwise, null.</returns>
    Task<Item?> FindAsync(int itemId);

    /// <summary>
    /// Checks if an item with the specified identifier exists in the database.
    /// </summary>
    /// <param name="itemId">The ID to check.</param>
    /// <returns>True if the item exists; otherwise, false.</returns>
    Task<bool> ExistsAsync(int itemId);

    /// <summary>
    /// Checks if an item name already exists in the database.
    /// </summary>
    /// <remarks>
    /// Used for validation to ensure unique naming across the inventory system.
    /// </remarks>
    /// <param name="name">The name to check.</param>
    /// <param name="excludeId">An optional ID to exclude from the check (common during updates).</param>
    /// <returns>True if a duplicate name is found; otherwise, false.</returns>
    Task<bool> ExistsAsync(string name, int? excludeId = null);

    /// <summary>
    /// Retrieves a detailed view of an item, including joined metadata from related tables.
    /// </summary>
    /// <param name="itemId">The unique ID of the item.</param>
    /// <returns>An <see cref="ItemDetailed"/> view object if found; otherwise, null.</returns>
    Task<ItemDetailed?> GetItemByIdAsync(int itemId);

    /// <summary>
    /// Retrieves a collection of all items formatted for list displays.
    /// </summary>
    /// <returns>A collection of <see cref="ItemList"/> objects.</returns>
    Task<IEnumerable<ItemList>> GetItemsAsync();

    /// <summary>
    /// Persists a new Item entity to the database.
    /// </summary>
    /// <param name="item">The item entity to add.</param>
    /// <returns>The newly assigned identifier for the item.</returns>
    Task<int> AddAsync(Item item);

    /// <summary>
    /// Updates an existing Item entity in the database.
    /// </summary>
    /// <param name="item">The item entity with updated values.</param>
    /// <returns>True if the database update was successful; otherwise, false.</returns>
    Task<bool> UpdateAsync(Item item);

    /// <summary>
    /// Removes an Item record from the database.
    /// </summary>
    /// <param name="item">The item entity to remove.</param>
    /// <returns>True if the deletion was successful; otherwise, false.</returns>
    Task<bool> DeleteAsync(Item item);

    /// <summary>
    /// Directly updates the 'UpdatedAt' timestamp for a specific item record in the database.
    /// </summary>
    /// <param name="itemId">The ID of the item to update.</param>
    /// <returns>The new <see cref="DateTime"/> value persisted to the database.</returns>
    Task<DateTime> ChangeUpdatedAtAsync(int itemId);
}