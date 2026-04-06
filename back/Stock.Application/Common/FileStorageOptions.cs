namespace Stock.Application.Common;

public class FileStorageOptions
{
    public const string SectionName = "FileStorage";
    public string TempPath { get; set; } = string.Empty;
    public string BoxPath { get; set; } = string.Empty;
    public string ItemPath { get; set; } = string.Empty;
}