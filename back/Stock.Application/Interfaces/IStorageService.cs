using Stock.Application.DTOs;

namespace Stock.Application.Interfaces;

/// <summary>
/// Defines the contract for managing the association between boxes, items, and brands.
/// This service handles inventory quantities, locations, and expiration tracking.
/// </summary>
public interface IStorageService
{
    /// <summary>
    /// Retrieves a list of all items currently stored inside a specific box.
    /// </summary>
    /// <param name="boxId">The unique identifier of the box container.</param>
    /// <returns>A collection of items found within the specified box.</returns>
    Task<IEnumerable<ItemInBoxListDto>> GetItemsByBoxIdAsync(int boxId);

    /// <summary>
    /// Removes the link between a specific item/brand combination and a box.
    /// </summary>
    /// <remarks>
    /// This "unbinds" the product from the container and recalculates the storage state.
    /// </remarks>
    /// <param name="boxId">The ID of the box.</param>
    /// <param name="itemId">The ID of the item.</param>
    /// <param name="brandId">The ID of the brand.</param>
    /// <returns>A status message or refreshed identifier representing the operation result.</returns>
    Task<string> UnbindBoxAndRefreshAsync(int boxId, int itemId, int brandId);

    /// <summary>
    /// Retrieves the precise storage details for a specific Box-Item-Brand triplet.
    /// </summary>
    /// <remarks>
    /// Used in the multi-step form (Step 2) to fetch current quantities and 
    /// check for existing records before an update.
    /// </remarks>
    /// <param name="boxId">The container box ID.</param>
    /// <param name="itemId">The product item ID.</param>
    /// <param name="brandId">The brand ID.</param>
    /// <returns>A <see cref="StorageDto"/> containing quantity and expiration metadata.</returns>
    Task<StorageDto> GetStorageAsync(int boxId, int itemId, int brandId);

    /// <summary>
    /// Retrieves all storage locations and quantities for a specific item across different boxes.
    /// </summary>
    /// <param name="itemId">The unique identifier of the item.</param>
    /// <returns>A collection of storage records where the item is present.</returns>
    Task<IEnumerable<ItemStorageListDto>> GetStorageByItemIdAsync(int itemId);

    /// <summary>
    /// Updates an existing storage record or creates a new one if it doesn't exist (Upsert).
    /// </summary>
    /// <remarks>
    /// Implementation should handle the logic for 'ExpiresOn' dates and validate 
    /// that quantities are not negative.
    /// </remarks>
    /// <param name="dto">The storage data transfer object to be saved.</param>
    /// <returns>True if the storage was successfully updated or created; otherwise, false.</returns>
    Task<bool> UpdateAsync(StorageDto dto);
}