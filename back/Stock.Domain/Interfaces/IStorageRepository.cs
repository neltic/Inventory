using Stock.Domain.Entities;
using Stock.Domain.Entities.Views;

namespace Stock.Domain.Interfaces;

/// <summary>
/// Repository interface for direct data access to Storage entities.
/// </summary>
public interface IStorageRepository
{
    /// <summary>
    /// Retrieves a list of items currently associated with a specific box from the database.
    /// </summary>
    /// <param name="boxId">The unique ID of the box container.</param>
    /// <returns>A collection of <see cref="ItemInBoxList"/> view objects.</returns>
    Task<IEnumerable<ItemInBoxList>> GetItemsByBoxIdAsync(int boxId);

    /// <summary>
    /// Removes the database relationship between a box and a specific item/brand triplet.
    /// </summary>    
    /// <param name="boxId">The ID of the box.</param>
    /// <param name="itemId">The ID of the item.</param>
    /// <param name="brandId">The ID of the brand.</param>
    /// <returns>A boolean representing the status of the removeving operation.</returns>
    Task<bool> RemoveAsync(int boxId, int itemId, int brandId);

    /// <summary>
    /// Finds a specific Storage entity based on the unique combination of Box, Item, and Brand.
    /// </summary>
    /// <param name="boxId">The ID of the box.</param>
    /// <param name="itemId">The ID of the item.</param>
    /// <param name="brandId">The ID of the brand.</param>
    /// <returns>The <see cref="Storage"/> entity if a record exists; otherwise, null.</returns>
    Task<Storage?> GetStorageAsync(int boxId, int itemId, int brandId);

    /// <summary>
    /// Retrieves all storage locations (boxes) where a specific item is currently stocked.
    /// </summary>
    /// <param name="itemId">The unique ID of the item to locate.</param>
    /// <returns>A collection of <see cref="ItemStorageList"/> view objects.</returns>
    Task<IEnumerable<ItemStorageList>> GetStorageByItemIdAsync(int itemId);

    /// <summary>
    /// Updates an existing storage record or persists a new one in the database.
    /// </summary>
    /// <remarks>
    /// This handles the persistence of quantities, expiration flags, and dates.
    /// </remarks>
    /// <param name="storage">The storage entity to be saved or updated.</param>
    /// <returns>True if the database operation was successful; otherwise, false.</returns>
    Task<bool> UpdateAsync(Storage storage);
}