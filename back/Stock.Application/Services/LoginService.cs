using Stock.Application.Interfaces;
using Stock.Application.Interfaces.Common;

namespace Stock.Application.Services;

public class LoginService(ITokenService tokenService) : ILoginService
{
    public string Login(string username, string password)
    {
        // For demonstration purposes
        if (username == "phi.avatar@gmail.com" && password == "password")
        {            
            return tokenService.Generate(username, "Admin");
        }
        else if (username == "phi.avatar@hotmail.com" && password == "password")
        {            
            return tokenService.Generate(username, "User");
        }

        return string.Empty;
    }
}
