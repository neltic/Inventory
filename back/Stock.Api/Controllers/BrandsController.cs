using Microsoft.AspNetCore.Mvc;
using Stock.Application.DTOs;
using Stock.Application.Interfaces;
using static Stock.Foundation.Common.LabelRegistry;

namespace Stock.Api.Controllers;

/// <summary>
/// Controller for managing product brands within the inventory system.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BrandsController(
    IBrandService BrandService,
    IGlobalizationService globalization) :
    ApiBaseController(globalization, Context.Brand)
{

    /// <summary>
    /// Retrieves all brands registered in the system.
    /// </summary>
    /// <returns>A collection of all available brands.</returns>
    /// <response code="200">Returns the list of brands.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<BrandDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BrandDto>>> GetBrands()
    {
        var result = await BrandService.GetAllAsync();
        return Ok(result);
    }

    // <summary>
    /// Retrieves the details of a specific brand by its unique identifier.
    /// </summary>
    /// <param name="id">The ID of the brand to retrieve.</param>
    /// <returns>Detailed information about the found brand.</returns>
    /// <response code="200">Returns the requested brand.</response>
    /// <response code="404">If a brand with the provided ID was not found.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BrandDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BrandDto>> GetById(int id)
    {
        var result = await BrandService.GetByIdAsync(id);

        if (result == null)
            return NotFound(new { message = Translate(Key.NotFound, id) });

        return Ok(result);
    }

    /// <summary>
    /// Updates the information of an existing brand.
    /// </summary>
    /// <param name="id">The ID of the brand to modify.</param>
    /// <param name="dto">The updated data for the brand.</param>
    /// <returns>A confirmation of the update operation.</returns>
    /// <response code="200">Brand updated successfully.</response>
    /// <response code="400">If the model data is invalid.</response>
    /// <response code="404">If the brand was not found.</response>
    /// <response code="409">If the update creates a conflict (e.g., duplicate brand name).</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] BrandDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var updated = await BrandService.UpdateAsync(id, dto);

            if (!updated)
                return NotFound(new { message = Translate(Key.CanNotUpdate, id) });

            return Ok(new { BrandId = id, updated });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Creates a new brand in the system.
    /// </summary>
    /// <param name="dto">The data for the new brand.</param>
    /// <returns>The ID of the newly created brand.</returns>
    /// <response code="200">Brand created successfully.</response>
    /// <response code="400">If the model data is invalid.</response>
    /// <response code="409">If a brand with the same name already exists.</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] BrandDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var newId = await BrandService.CreateAsync(dto);
            return Ok(new { BrandId = newId });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Deletes a brand from the system by its ID.
    /// </summary>
    /// <remarks>
    /// Deletion will fail if the brand is currently associated with any products (referential integrity).
    /// </remarks>
    /// <param name="id">The ID of the brand to delete.</param>
    /// <returns>NoContent if the operation was successful.</returns>
    /// <response code="204">Brand deleted successfully.</response>
    /// <response code="409">If the brand cannot be deleted due to existing dependencies.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await BrandService.DeleteAsync(id);

        if (!deleted)
            return Conflict(new { message = Translate(Key.CanNotDelete, id) });

        return NoContent();
    }
}
