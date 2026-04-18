using Microsoft.AspNetCore.Mvc;
using Stock.Application.DTOs;
using Stock.Application.Interfaces;

namespace Stock.Api.Controllers;

/// <summary>
/// Handles globalization and localization operations, providing translation resources to the client applications.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GlobalizationController(IGlobalizationService globalization) : ControllerBase
{
    /// <summary>
    /// Retrieves all available languages for the application.
    /// </summary>
    /// <returns>A list of languages including their codes and names.</returns>
    /// <response code="200">Returns the list of supported languages.</response>
    [HttpGet("languages")]
    [ProducesResponseType(typeof(IEnumerable<LanguageDto>), 200)]
    public async Task<ActionResult<IEnumerable<LanguageDto>>> GetAllLanguages()
    {
        var result = await globalization.GetAllLanguagesAsync();
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a structured dictionary of translations for a specific language.
    /// </summary>
    /// <returns>A JSON object containing translations grouped by context and keys.</returns>
    /// <response code="200">Returns the translation dictionary. If the requested language is missing, the default language is returned.</response>
    [HttpGet("locales")]
    [ProducesResponseType(typeof(IDictionary<string, IDictionary<string, string>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLocales()
    {
        var translations = await globalization.GetLanguageDictionaryAsync();
        return Ok(translations);
    }

    /// <summary>
    /// Synchronizes and reloads the translation cache from the database.
    /// </summary>
    /// <remarks>
    /// This operation updates both the in-memory Storage (Singleton) and the distributed cache (Redis).
    /// Should be triggered after manual updates to translation tables or language settings.
    /// </remarks>
    /// <returns>A confirmation message indicating the cache has been refreshed.</returns>
    /// <response code="200">Cache successfully refreshed.</response>
    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Refresh()
    {
        await globalization.InitializeCacheAsync();
        return Ok(new { message = "Cache refreshed" });
    }
}
