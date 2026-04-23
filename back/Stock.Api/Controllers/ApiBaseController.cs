using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Interfaces;

namespace Stock.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ApiBaseController(IGlobalizationService globalization, string context) : ControllerBase
{
    protected string Translate(string key, params object[] values)
        => globalization.Translate(context, key, values);
}
