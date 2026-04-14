namespace Stock.Worker.Common;

public class GoogleDriveOptions
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string AppName { get; set; } = string.Empty;
    public string RootFolderId { get; set; } = string.Empty;
}