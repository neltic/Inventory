namespace Stock.Application.Interfaces.Common;

/// <summary>
/// Defines the contract for handling file operations, specifically managing 
/// image storage and their assignment to system entities.
/// </summary>
public interface IFileStorageService
{
    /// <summary>
    /// Saves an uploaded image stream to a temporary storage location.
    /// </summary>
    /// <remarks>
    /// This is the first step in the upload process. It generates a unique identifier 
    /// (GUID) to track the file before it is permanently linked to an entity.
    /// </remarks>
    /// <param name="fileStream">The binary stream of the image file.</param>
    /// <returns>A unique file name or GUID representing the temporary file.</returns>
    Task<string> SaveTempImageAsync(Stream fileStream);

    /// <summary>
    /// Moves a temporary image to permanent storage and links it to a specific box.
    /// </summary>
    /// <remarks>
    /// This operation typically involves renaming or relocating the physical file 
    /// and updating the Box record in the database.
    /// </remarks>
    /// <param name="tempFileName">The identifier of the temporary file returned by <see cref="SaveTempImageAsync"/>.</param>
    /// <param name="boxId">The ID of the box to which the image will be assigned.</param>
    /// <returns>True if the assignment was successful; otherwise, false.</returns>
    Task<bool> AssignImageToBoxAsync(string tempFileName, int boxId);

    /// <summary>
    /// Moves a temporary image to permanent storage and links it to a specific item.
    /// </summary>
    /// <remarks>
    /// Similar to the box assignment, this moves the file to the item's permanent 
    /// storage path and updates the item's image metadata.
    /// </remarks>
    /// <param name="tempFileName">The identifier of the temporary file returned by <see cref="SaveTempImageAsync"/>.</param>
    /// <param name="itemId">The ID of the item to which the image will be assigned.</param>
    /// <returns>True if the assignment was successful; otherwise, false.</returns>
    Task<bool> AssignImageToItemAsync(string tempFileName, int itemId);

    /// <summary>
    /// Moves all images associated with a box (original, thumbnails, icons) to the temporary storage for deferred deletion.
    /// </summary>
    /// <param name="boxId">The unique identifier of the box.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteBoxImagesAsync(int boxId);

    /// <summary>
    /// Moves all images associated with an item (original, thumbnails, icons) to the temporary storage for deferred deletion.
    /// </summary>
    /// <param name="itemId">The unique identifier of the item.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteItemImagesAsync(int itemId);
}