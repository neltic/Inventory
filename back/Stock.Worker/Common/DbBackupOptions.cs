namespace Stock.Worker.Common;

public class DbBackupOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string StoragePath { get; set; } = string.Empty;
}
