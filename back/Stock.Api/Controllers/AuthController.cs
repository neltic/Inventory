using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Interfaces;
using Stock.Foundation.Common;
using static Stock.Foundation.Common.LabelRegistry;

namespace Stock.Api.Controllers;

public class AuthController(
    IGlobalizationService globalization) :
    ApiBaseController(globalization, Context.Auth)
{
    [HttpGet("test")]
    [Authorize(Roles = RoleRealm.Admin)]
    public IActionResult Test()
    {
        return Ok(User.Claims.Select(c => new { c.Type, c.Value }));
    }
}
