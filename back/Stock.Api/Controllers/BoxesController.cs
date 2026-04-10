using Microsoft.AspNetCore.Mvc;
using Stock.Application.DTOs;
using Stock.Application.Interfaces;
using Stock.Application.Interfaces.Common;
using Stock.Application.Services;
using Stock.Domain.Entities.Views;

namespace Stock.Api.Controllers;

/// <summary>
/// Controller for managing boxes.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BoxesController(IBoxService boxService, IFileStorageService fileService) : ControllerBase
{

    /// <summary>
    /// Retrieves a list of boxes filtered by their parent container.
    /// </summary>
    /// <param name="parentBoxId">The ID of the parent box. If null, it retrieves root-level boxes.</param>
    /// <returns>A collection of boxes found within the specified level.</returns>
    /// <response code="200">Returns the list of boxes.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BoxListDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BoxListDto>>> GetByParent([FromQuery] int? parentBoxId)
    {
        var result = await boxService.GetBoxesByParentBoxIdAsync(parentBoxId);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a simplified list of all boxes for selection purposes (Lookup).
    /// </summary>
    /// <remarks>
    /// This endpoint is optimized for populating selectors in the frontend without loading heavy data.
    /// </remarks>
    /// <returns>A list of boxes containing basic info.</returns>
    [HttpGet("lookup")]
    [ProducesResponseType(typeof(IEnumerable<BoxLookupListDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BoxLookupListDto>>> GetBoxesLookup()
    {
        var result = await boxService.GetBoxesLookupAsync();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves the full details of a specific box by its identifier.
    /// </summary>
    /// <param name="id">The ID of the box to retrieve.</param>
    /// <returns>Detailed information about the found box.</returns>
    /// <response code="200">Returns the requested box.</response>
    /// <response code="404">If a box with the provided ID was not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BoxDetailedDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BoxDetailedDto>> GetById(int id)
    {
        var result = await boxService.GetBoxByIdAsync(id);

        if (result == null)
            return NotFound(new { message = $"Box with ID {id} was not found." });

        return Ok(result);
    }

    /// <summary>
    /// Searches for and returns an empty box within a specific parent container.
    /// </summary>
    /// <param name="parentBoxId">The ID of the parent box to search within.</param>
    /// <returns>Details of the empty box.</returns>
    [HttpGet("empty")]
    [ProducesResponseType(typeof(BoxDetailedDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<BoxDetailedDto>> GetEmptyByParentBoxId([FromQuery] int? parentBoxId)
    {
        var result = await boxService.GetEmptyBoxByParentBoxIdAsync(parentBoxId);

        return Ok(result);
    }

    /// <summary>
    /// Retrieves the full hierarchical path (breadcrumb) of a specific box's parents.
    /// </summary>
    /// <param name="id">The unique identifier of the box.</param>
    /// <returns>A JSON array representing the chain of parent boxes from Root to the current location.</returns>
    /// <response code="200">Returns the JSON path string.</response>
    /// <response code="404">If the path cannot be found or the box is at the Root level.</response>
    [HttpGet("{id}/path")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBoxFullPath(int id)
    {
        var jsonPath = await boxService.GetBoxFullPathAsync(id);

        if (string.IsNullOrEmpty(jsonPath))
        {
            return Ok("[]");
        }

        return Content(jsonPath, "application/json");
    }

    /// <summary>
    /// Retrieves a hierarchical list of potential parent boxes.
    /// Use without an ID for new box creation, or with an ID to move an existing box.
    /// </summary>
    /// <param name="targetBoxId">The optional ID of the box to be moved.</param>
    [HttpGet("available-parents/{targetBoxId?}")] 
    [ProducesResponseType(typeof(IEnumerable<BoxTransferList>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BoxTransferList>>> GetAvailableParents(int? targetBoxId)
    {        
        var availableParents = await boxService.GetAvailableParentBoxesByAsync(targetBoxId);

        return Ok(availableParents);
    }

    /// <summary>
    /// Creates a new box in the system.
    /// </summary>
    /// <param name="dto">The data for the new box.</param>
    /// <returns>The ID of the newly created box.</returns>
    /// <response code="200">Box created successfully.</response>
    /// <response code="400">If the model data is invalid.</response>
    /// <response code="409">If a business logic conflict occurs (e.g., duplicate name at the same hierarchy level).</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] BoxDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var newId = await boxService.CreateAsync(dto);
            return Ok(new { boxId = newId });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Updates an existing box's information.
    /// </summary>
    /// <param name="id">The ID of the box to modify.</param>
    /// <param name="dto">The updated data for the box.</param>
    /// <returns>A confirmation of the update.</returns>
    /// <response code="200">Box updated successfully.</response>
    /// <response code="404">If the box was not found or could not be updated.</response>
    /// <response code="409">If the update creates a conflict with existing records.</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] BoxDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var updated = await boxService.UpdateAsync(id, dto);

            if (!updated)
                return NotFound(new { message = $"Can not update box with ID {id}." });

            return Ok(new { boxId = id, updated });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Deletes a box from the system by its ID.
    /// </summary>
    /// <remarks>
    /// Deletion may fail (409 Conflict) if the box contains other boxes or stored items (integrity constraint).
    /// </remarks>
    /// <param name="id">The ID of the box to delete.</param>
    /// <returns>NoContent if the operation was successful.</returns>
    /// <response code="204">Box deleted successfully.</response>
    /// <response code="409">If the box cannot be deleted due to existing dependencies.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await boxService.DeleteAsync(id);

        if (!deleted)
            return Conflict(new { message = $"Can not delete box with ID {id}." });

        await fileService.DeleteBoxImagesAsync(id);

        return NoContent();
    }

    /// <summary>
    /// Assigns a previously uploaded image to a specific box.
    /// </summary>
    /// <remarks>
    /// This process links a temporary file (identified by fileGuid) with the box record 
    /// and refreshes the box's update timestamp.
    /// </remarks>
    /// <param name="id">The ID of the box.</param>
    /// <param name="fileGuid">The unique identifier of the stored physical file.</param>
    /// <returns>The new updated timestamp of the box.</returns>
    /// <response code="200">Image assigned successfully.</response>
    /// <response code="500">Internal error while processing file assignment.</response>
    [HttpPost("{id}/assign-image/{fileGuid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AssignImage(int id, string fileGuid)
    {
        try
        {
            var processed = await fileService.AssignImageToBoxAsync(fileGuid, id);
            if (!processed) throw new Exception("Failed to assign the image to the box.");

            var updatedAt = await boxService.ChangeUpdatedAtAsync(id);

            return Ok(new { updatedAt });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal error processing the image: {ex.Message}");
        }
    }

    /// <summary>
    /// Changes the parent container of a specific box.
    /// </summary>
    /// <param name="id">The unique identifier of the box to be moved.</param>
    /// <param name="newParentId">
    /// The ID of the destination box. 
    /// If omitted, the box will be moved to the system root level.
    /// </param>
    /// <returns>A 204 No Content response if the move is successful.</returns>
    /// <response code="204">The box was successfully moved to its new location.</response>
    /// <response code="400">If the move is invalid (e.g., circular reference or illegal parent).</response>
    [HttpPatch("{id}/move-to/{newParentId?}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> MoveBox(int id, int? newParentId)
    {
        try
        {            
            await boxService.MoveBoxAsync(id, newParentId);
            return NoContent();
        }
        catch (Exception ex)
        {            
            return BadRequest(new { ex.Message });
        }
    }
}
