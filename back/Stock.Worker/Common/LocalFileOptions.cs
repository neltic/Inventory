namespace Stock.Worker.Common;

public class LocalFileOptions
{
    public string StoragePath { get; set; } = string.Empty;
    public bool RunAtStartup { get; set; } = true;
    public int RunAfterMinutes { get; set; } = 60;
}