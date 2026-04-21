using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Stock.Application.Common;
using Stock.Application.Interfaces.Common;

namespace Stock.Infrastructure.Services;

public class TokenService(IOptions<TokenOptions> options) : ITokenService
{
    private readonly TokenOptions _options = options.Value;

    public string Generate(string email, string role)
    {        
        var claims = new Claim[] {
            new(JwtRegisteredClaimNames.Email, email),
            new(ClaimTypes.Role, role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                
        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
            signingCredentials: creds);
                
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
