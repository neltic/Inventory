namespace Stock.Application.Common;

public class CurrentUserInfo
{
    public string? UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? Email { get; set; }
    public List<string> Roles { get; set; } = [];
    public string? ClientId { get; set; }
    public string? SessionId { get; set; }
}
