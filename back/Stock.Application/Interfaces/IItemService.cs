using Stock.Application.DTOs;

namespace Stock.Application.Interfaces;

/// <summary>
/// Defines the contract for managing inventory items, including their metadata and lifecycle.
/// </summary>
public interface IItemService
{
    /// <summary>
    /// Retrieves full details of a specific item by its identifier.
    /// </summary>
    /// <param name="itemId">The unique ID of the item.</param>
    /// <returns>An <see cref="ItemDetailedDto"/> if found; otherwise, null.</returns>
    Task<ItemDetailedDto?> GetItemByIdAsync(int itemId);

    /// <summary>
    /// Retrieves a new item template with default initialized values.
    /// </summary>
    /// <remarks>
    /// This is typically used to pre-populate creation forms or provide a clean state for the UI.
    /// </remarks>
    /// <returns>A <see cref="ItemDetailedDto"/> representing an empty item state.</returns>
    Task<ItemDetailedDto?> GetEmptyItemAsync();

    /// <summary>
    /// Retrieves a list of all items currently registered in the inventory.
    /// </summary>
    /// <returns>A collection of items formatted for list views.</returns>
    Task<IEnumerable<ItemListDto>> GetItemsAsync();

    /// <summary>
    /// Retrieves a collection of storage locations where a specific item is currently stocked.
    /// </summary>
    /// <param name="itemId">The unique identifier of the item to locate.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains an enumerable of <see cref="ItemLocationListDto"/> 
    /// detailing the boxes, brands, and quantities associated with the item.
    /// </returns>
    /// <remarks>
    /// This method performs a join between the Storage and Box entities to provide 
    /// descriptive location data along with stock quantities.
    /// </remarks>
    Task<IEnumerable<ItemLocationListDto>> GetItemLocationAsync(int itemId);

    /// <summary>
    /// Creates a new item record.
    /// </summary>
    /// <param name="dto">The item data to be created.</param>
    /// <returns>The unique identifier of the newly created item.</returns>
    /// <exception cref="InvalidOperationException">Thrown if business validation fails (e.g., duplicate SKUs or names).</exception>
    Task<int> CreateAsync(ItemDto dto);

    /// <summary>
    /// Updates the details of an existing item.
    /// </summary>
    /// <param name="itemId">The ID of the item to update.</param>
    /// <param name="dto">The updated item information.</param>
    /// <returns>True if the update was successful; otherwise, false.</returns>
    Task<bool> UpdateAsync(int itemId, ItemDto dto);

    /// <summary>
    /// Deletes an item from the system.
    /// </summary>
    /// <remarks>
    /// Deletion will fail if the item is currently linked to active storage or historical transactions.
    /// </remarks>
    /// <param name="itemId">The ID of the item to remove.</param>
    /// <returns>True if the deletion was successful; otherwise, false.</returns>
    Task<bool> DeleteAsync(int itemId);

    /// <summary>
    /// Refreshes the last modification timestamp for a specific item.
    /// </summary>
    /// <remarks>
    /// Used to track changes when related entities (like images) are updated without modifying the main item record.
    /// </remarks>
    /// <param name="itemId">The ID of the item to update.</param>
    /// <returns>The updated <see cref="DateTime"/> value.</returns>
    Task<DateTime> ChangeUpdatedAtAsync(int itemId);
}