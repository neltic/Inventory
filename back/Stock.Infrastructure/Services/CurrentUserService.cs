using Microsoft.AspNetCore.Http;
using Stock.Application.Common;
using Stock.Application.Interfaces.Common;
using System.Security.Claims;

namespace Stock.Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public string Username
    {
        get
        {
            var name = httpContextAccessor.HttpContext?.User?.FindFirstValue("preferred_username");

            if (string.IsNullOrEmpty(name))
            {
                throw new UnauthorizedAccessException("Attempted operation without valid user identity.");
            }

            return name;
        }
    }

    public CurrentUserInfo GetInfo()
    {
        var user = httpContextAccessor.HttpContext?.User;

        return new CurrentUserInfo
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier),
            Username = Username,
            Email = user?.FindFirstValue(ClaimTypes.Email),
            Roles = user?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList() ?? [],
            ClientId = user?.FindFirstValue("azp"),
            SessionId = user?.FindFirstValue("sid")
        };
    }
}
