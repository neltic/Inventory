namespace Stock.Application.Interfaces.Common;

public interface ITokenService
{
    string Generate(string email, string role);
}
