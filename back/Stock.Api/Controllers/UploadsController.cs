using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Interfaces.Common;
using Stock.Foundation.Common;

namespace Stock.Api.Controllers;

/// <summary>
/// Controller for handling temporary file and image uploads.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UploadsController(IFileStorageService fileService) : ControllerBase
{
    /// <summary>
    /// Uploads an image to temporary storage.
    /// </summary>
    /// <remarks>
    /// This method saves the file stream and returns a unique GUID. 
    /// This GUID should then be used with the "assign-image" endpoints 
    /// in the Boxes or Items controllers to permanently link the file.
    /// </remarks>
    /// <param name="file">The image file from the multipart/form-data request.</param>
    /// <returns>A unique identifier (fileGuid) for the stored temporary file.</returns>
    /// <response code="200">Returns the GUID of the uploaded image.</response>
    /// <response code="400">If the file is null or the request is invalid.</response>
    [HttpPost("image")]
    [Authorize(Roles = RoleRealm.Operator)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null) return BadRequest();

        var fileGuid = await fileService.SaveTempImageAsync(file.OpenReadStream());

        return Ok(new { fileGuid });
    }
}
