using Microsoft.AspNetCore.Mvc;
using Stock.Application.DTOs;
using Stock.Application.Interfaces;

namespace Stock.Api.Controllers;

/// <summary>
/// Controller for managing the association between boxes, items, and brands (Inventory Storage).
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class StoragesController(IStorageService storageService) : ControllerBase
{
    /// <summary>
    /// Retrieves storage information filtered by a specific item.
    /// </summary>
    /// <param name="itemId">The ID of the item to search for.</param>
    /// <returns>The storage details associated with the item.</returns>
    /// <response code="200">Returns the item's storage data.</response>
    [HttpGet("items/{itemId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStorageByItem(int itemId)
    {
        var storage = await storageService.GetStorageByItemIdAsync(itemId);

        return Ok(storage);
    }

    /// <summary>
    /// Retrieves a list of all items currently stored inside a specific box.
    /// </summary>
    /// <param name="boxId">The ID of the box container.</param>
    /// <returns>A collection of items found within the box.</returns>
    /// <response code="200">Returns the list of items in the box.</response>
    /// <response code="404">If the box is empty or not found.</response>
    [HttpGet("boxes/{boxId}")]
    [ProducesResponseType(typeof(IEnumerable<ItemInBoxListDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IEnumerable<ItemInBoxListDto>>> GetItemsByBoxId(int boxId)
    {
        var result = await storageService.GetItemsByBoxIdAsync(boxId);

        if (result == null)
            return NotFound(new { message = $"There are no items in that box with ID {boxId}." });

        return Ok(result);
    }

    /// <summary>
    /// Retrieves specific storage details for a unique combination of Box, Item, and Brand.
    /// </summary>
    /// <remarks>
    /// This is used in the multi-step form to pre-load existing quantities and expiration data.
    /// </remarks>
    /// <param name="boxId">The container box ID.</param>
    /// <param name="itemId">The product item ID.</param>
    /// <param name="brandId">The brand ID.</param>
    /// <returns>Storage details including quantity and expiration info.</returns>
    /// <response code="200">Returns the specific storage record.</response>
    [HttpGet("boxes/{boxId}/items/{itemId}/brands/{brandId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStorage(int boxId, int itemId, int brandId)
    {
        var result = await storageService.GetStorageAsync(boxId, itemId, brandId);

        return Ok(result);
    }

    /// <summary>
    /// Updates or creates storage information (Upsert).
    /// </summary>
    /// <remarks>
    /// Handles updating quantities and expiration dates. 
    /// If 'Expires' is true, 'ExpiresOn' must be provided in "yyyy-MM-dd" format.
    /// </remarks>
    /// <param name="dto">The storage data transfer object.</param>
    /// <returns>Confirmation of the update status.</returns>
    /// <response code="200">Storage updated successfully.</response>
    /// <response code="400">If the model state is invalid.</response>
    /// <response code="404">If the storage record could not be found or updated.</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStorage([FromBody] StorageDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updated = await storageService.UpdateAsync(dto);

        if (!updated)
            return NotFound(new { message = $"Can not update storage.", updated });

        return Ok(new { updated });
    }

    /// <summary>
    /// Removes the association between an item/brand and a box (Unbind).
    /// </summary>
    /// <remarks>
    /// This effectively removes the item from the box and refreshes the storage state.
    /// </remarks>
    /// <param name="boxId">The ID of the box.</param>
    /// <param name="itemId">The ID of the item.</param>
    /// <param name="brandId">The ID of the brand.</param>
    /// <returns>The result of the unbinding operation.</returns>
    /// <response code="200">The association was successfully removed.</response>
    [HttpDelete("boxes/{boxId}/items/{itemId}/brands/{brandId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UnbindBoxAndRefresh(int boxId, int itemId, int brandId)
    {
        var result = await storageService.UnbindBoxAndRefreshAsync(boxId, itemId, brandId);

        return Ok(result);
    }
}
