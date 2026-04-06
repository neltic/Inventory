using Microsoft.AspNetCore.Mvc;
using Stock.Application.DTOs;
using Stock.Application.Interfaces;

namespace Stock.Api.Controllers;

/// <summary>
/// Controller for managing product categories and their display order.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    /// <summary>
    /// Retrieves all categories registered in the system.
    /// </summary>
    /// <returns>A collection of categories.</returns>
    /// <response code="200">Returns the list of categories.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        var result = await categoryService.GetAllAsync();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves the details of a specific category by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the category to retrieve.</param>
    /// <returns>Detailed information about the found category.</returns>
    /// <response code="200">Returns the requested category.</response>
    /// <response code="404">If the category was not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CategoryDto>> GetById(int id)
    {
        var result = await categoryService.GetByIdAsync(id);

        if (result == null)
            return NotFound(new { message = $"Category with ID {id} was not found." });

        return Ok(result);
    }

    /// <summary>
    /// Updates the information of an existing category.
    /// </summary>
    /// <param name="id">The ID of the category to modify.</param>
    /// <param name="dto">The updated data for the category.</param>
    /// <returns>Confirmation of the update operation.</returns>
    /// <response code="200">Category updated successfully.</response>
    /// <response code="400">If the model data is invalid.</response>
    /// <response code="404">If the category does not exist.</response>
    /// <response code="409">If the update creates a conflict (e.g., duplicate name).</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] CategoryDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var updated = await categoryService.UpdateAsync(id, dto);

            if (!updated)
                return NotFound(new { message = $"Can not update category with ID {id}." });

            return Ok(new { categoryId = id, updated });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Creates a new category in the system.
    /// </summary>
    /// <param name="dto">The data for the new category.</param>
    /// <returns>The ID of the newly created category.</returns>
    /// <response code="200">Category created successfully.</response>
    /// <response code="400">If the model data is invalid.</response>
    /// <response code="409">If a category with the same name already exists.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CategoryDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var newId = await categoryService.CreateAsync(dto);
            return Ok(new { categoryId = newId });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Deletes a category from the system.
    /// </summary>
    /// <remarks>
    /// Deletion will fail if there are items associated with this category due to referential integrity.
    /// </remarks>
    /// <param name="id">The ID of the category to delete.</param>
    /// <returns>NoContent if the operation was successful.</returns>
    /// <response code="204">Category deleted successfully.</response>
    /// <response code="409">If the category cannot be deleted due to existing dependencies.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await categoryService.DeleteAsync(id);

        if (!deleted)
            return Conflict(new { message = $"Can not delete category with ID {id}." });

        return NoContent();
    }

    /// <summary>
    /// Updates the display order of a specific category.
    /// </summary>
    /// <remarks>
    /// This method shifts the order of other categories to accommodate the new position 
    /// of the specified category ID. Useful for drag-and-drop interfaces.
    /// </remarks>
    /// <param name="id">The ID of the category to reorder.</param>
    /// <param name="newOrder">The new index/position for the category.</param>
    /// <returns>The updated status of the reordering.</returns>
    /// <response code="200">Returns the updated status of the reordering.</response>
    /// <response code="400">If the request parameters are invalid.</response>
    /// <response code="409">If the reordering logic fails due to business constraints.</response>
    [HttpPatch("{id}/reorder/{newOrder}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Reorder(int id, int newOrder)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var updated = await categoryService.ReorderAsync(id, newOrder);
            return Ok(new { categoryId = id, updated });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}
