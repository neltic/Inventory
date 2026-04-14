using Microsoft.AspNetCore.Mvc;
using Stock.Application.DTOs;
using Stock.Application.Interfaces;
using Stock.Application.Interfaces.Common;

namespace Stock.Api.Controllers;

/// <summary>
/// Controller for managing inventory items.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ItemsController(IItemService itemService, IFileStorageService fileService) : ControllerBase
{
    /// <summary>
    /// Retrieves a list of all items in the inventory.
    /// </summary>
    /// <returns>A collection of items with basic information.</returns>
    /// <response code="200">Returns the list of items.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ItemListDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ItemListDto>>> GetItems()
    {
        var result = await itemService.GetItemsAsync();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves full details of a specific item by its identifier.
    /// </summary>
    /// <param name="id">The unique ID of the item.</param>
    /// <returns>Detailed information about the item.</returns>
    /// <response code="200">Returns the requested item details.</response>
    /// <response code="404">If the item ID does not exist.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ItemDetailedDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ItemDetailedDto>> GetById(int id)
    {
        var item = await itemService.GetItemByIdAsync(id);
        return item != null ? Ok(item) : NotFound(new { message = $"Item with ID {id} was not found." });
    }

    /// <summary>
    /// Retrieves a template or the empty item record.
    /// </summary>
    /// <returns>Information about an empty item.</returns>
    /// <response code="200">Returns the empty item data.</response>
    [HttpGet("empty")]
    [ProducesResponseType(typeof(BoxDetailedDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<BoxDetailedDto>> GetEmptyItem()
    {
        var result = await itemService.GetEmptyItemAsync();

        return Ok(result);
    }

    /// <summary>
    /// Retrieves the storage locations and quantities for a specific item.
    /// </summary>
    /// <param name="itemId">The unique identifier of the item.</param>
    /// <returns>
    /// A list of <see cref="ItemLocationListDto"/> representing the storage locations.
    /// Returns an empty list if no locations are found.
    /// </returns>
    /// <response code="200">Returns the list of locations (can be empty).</response>
    [HttpGet("{itemId}/locations")]
    [ProducesResponseType(typeof(IEnumerable<ItemLocationListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetItemLocations(int itemId)
    {
        var locations = await itemService.GetItemLocationAsync(itemId);

        return Ok(locations ?? Enumerable.Empty<ItemLocationListDto>());
    }

    /// <summary>
    /// Creates a new item in the system.
    /// </summary>
    /// <param name="dto">The data for the new item.</param>
    /// <returns>The ID of the newly created item.</returns>
    /// <response code="200">Item created successfully.</response>
    /// <response code="400">If the provided data is invalid.</response>
    /// <response code="409">If a business conflict occurs (e.g., item already exists).</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] ItemDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var newId = await itemService.CreateAsync(dto);
            return Ok(new { itemId = newId });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Updates the details of an existing item.
    /// </summary>
    /// <param name="id">The ID of the item to modify.</param>
    /// <param name="dto">The updated item data.</param>
    /// <returns>Confirmation of the update.</returns>
    /// <response code="200">Item updated successfully.</response>
    /// <response code="400">If the request data is invalid.</response>
    /// <response code="404">If the item to update was not found.</response>
    /// <response code="409">If the update violates system constraints.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] ItemDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var updated = await itemService.UpdateAsync(id, dto);

            if (!updated)
                return NotFound(new { message = $"Can not update item with ID {id}." });

            return Ok(new { itemId = id, updated });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Deletes an item from the system.
    /// </summary>
    /// <remarks>
    /// Deletion will fail if the item is currently referenced in active storage or transactions.
    /// </remarks>
    /// <param name="id">The ID of the item to delete.</param>
    /// <returns>NoContent if successful.</returns>
    /// <response code="204">Item deleted successfully.</response>
    /// <response code="409">If the item cannot be deleted due to dependencies.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id)
    {

        var deleted = await itemService.DeleteAsync(id);

        if (!deleted)
            return Conflict(new { message = $"Can not delete item with ID {id}." });

        await fileService.DeleteItemImagesAsync(id);

        return NoContent();
    }

    /// <summary>
    /// Links a previously uploaded image file to a specific item.
    /// </summary>
    /// <remarks>
    /// This method associates a file via its unique GUID and updates the item's last modification timestamp.
    /// </remarks>
    /// <param name="id">The ID of the item.</param>
    /// <param name="fileGuid">The GUID of the physical file in storage.</param>
    /// <returns>The updated timestamp of the item.</returns>
    /// <response code="200">Image successfully linked to the item.</response>
    /// <response code="500">Internal error during file processing or assignment.</response>
    [HttpPost("{id}/assign-image/{fileGuid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AssignImage(int id, string fileGuid)
    {
        try
        {
            var processed = await fileService.AssignImageToItemAsync(fileGuid, id);
            if (!processed) throw new Exception("Failed to assign the image to the item.");

            var updatedAt = await itemService.ChangeUpdatedAtAsync(id);

            return Ok(new { updatedAt });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal error processing the image: {ex.Message}");
        }
    }
}