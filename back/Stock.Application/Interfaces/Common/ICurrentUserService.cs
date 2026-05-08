using Stock.Application.Common;

namespace Stock.Application.Interfaces.Common;

public interface ICurrentUserService
{
    string Username { get; }

    CurrentUserInfo GetInfo();
}
