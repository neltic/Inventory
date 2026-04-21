using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Interfaces;
using static Stock.Foundation.Common.LabelRegistry;

namespace Stock.Api.Controllers;

/// <summary>
/// Controller for managing authentication and authorization.
/// </summary>
public class AuthController(
    ILoginService loginService,
    IGlobalizationService globalization) :
    ApiBaseController(globalization, Context.Auth)
{
    /// <summary>
    /// Authenticates a user based on the provided login credentials and returns a JWT token if authentication is
    /// successful.
    /// </summary>
    /// <param name="request">The login credentials containing the user's email and password.</param>
    /// <returns>An HTTP 200 response with a JWT token if authentication succeeds; otherwise, an HTTP 401 Unauthorized response.</returns>
    [HttpPost("login")]    
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var token = loginService.Login(request.Email, request.Password);

        if (string.IsNullOrEmpty(token)) return Unauthorized();

        return Ok(new { Token = token });
    }
}
