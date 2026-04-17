using Microsoft.AspNetCore.Mvc;
using Stock.Application.Interfaces;

namespace Stock.Api.Controllers;

/// <summary>
/// Handles globalization and localization operations, providing translation resources to the client applications.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class GlobalizationController(IGlobalizationService globalizationService) : ControllerBase
{
    /// <summary>
    /// Retrieves a structured dictionary of translations for a specific language.
    /// </summary>
    /// <param name="lang">The ISO 639-1 language code (e.g., "en", "es").</param>
    /// <returns>A JSON object containing translations grouped by context and keys.</returns>
    /// <response code="200">Returns the translation dictionary. If the requested language is missing, the default language is returned.</response>
    [HttpGet("locales/{lang}")]
    [ProducesResponseType(typeof(IDictionary<string, IDictionary<string, string>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLocales(string lang)
    {
        var translations = await globalizationService.GetLanguageJsonAsync(lang);
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
    /// <response code="500">Internal error during cache synchronization or database connectivity issues.</response>
    [HttpPost("refresh")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Refresh()
    {
        await globalizationService.InitializeCacheAsync();
        return Ok(new { message = "Cache refreshed" });
    }
}
