using Stock.Worker.Common;
using Stock.Worker.Interfaces;

namespace Stock.Worker.Services;

public class LocalFileService(LocalFileOptions options) : ILocalFileService
{
    private const string IMAGE_FOLDER_NAME = "img";
    private const string TEMP_FOLDER_NAME = "temp";
    private const string DEFAULT_IMG_EXTENSION = ".png";
    private const string DEFAULT_SYNC_EXTENSION = ".txt";
    private readonly string StaticPath = GetLocalStaticPath(options.StoragePath);

    public string GetStaticPath() => StaticPath;

    public string GetImagePath() => Path.Combine(StaticPath, IMAGE_FOLDER_NAME);

    public void CheckStaticDirectory()
    {
        CheckDirectory(StaticPath);
    }

    public string[] GetFiles()
    {
        return Directory.GetFiles(GetImagePath(), "*.*", SearchOption.AllDirectories);
    }

    public bool TryGetPendingSyncFiles(string syncFileName, out string[] pendingFiles)
    {
        var parts = syncFileName.Replace(DEFAULT_SYNC_EXTENSION, "", StringComparison.OrdinalIgnoreCase).Split('_');

        if(parts.Length < 2)
        {
            pendingFiles = [];
            return false;
        }

        string origin = parts[1];
        string id = parts[2];

        string[] subFolders = ["original", "thumbnails", "icons"];
        pendingFiles = [.. subFolders.Select(sub => Path.Combine(GetImagePath(), origin, sub, $"{id}{DEFAULT_IMG_EXTENSION}"))];

        return true;
    }

    public string GetRelativeToStaticPath(string file)
    {
        return Path.GetRelativePath(StaticPath, file);
    }

    public bool IsRemovable(string relativePath, out string fileName)
    {
        fileName = Path.GetFileName(relativePath);
        string normalizedPath = GetNormalizedPath(relativePath);
        return normalizedPath.StartsWith($"{TEMP_FOLDER_NAME}/", StringComparison.OrdinalIgnoreCase) &&
               fileName.StartsWith("deleted_", StringComparison.OrdinalIgnoreCase);
    }

    public bool IsInImageFolder(string relativePath)
    {
        string normalizedPath = GetNormalizedPath(relativePath);
        return normalizedPath.StartsWith($"{IMAGE_FOLDER_NAME}/", StringComparison.OrdinalIgnoreCase);
    }

    public bool IsSyncFile(string relativePath, out string fileName)
    {
        fileName = Path.GetFileName(relativePath);
        string normalizedPath = GetNormalizedPath(relativePath);
        return normalizedPath.StartsWith($"{TEMP_FOLDER_NAME}/", StringComparison.OrdinalIgnoreCase) &&
               fileName.StartsWith("sync_", StringComparison.OrdinalIgnoreCase) &&
               fileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase);
    }

    public bool TryGetFileNameAndParts(string fileName, out string originalFileName, out string[] pathParts)
    {
        var parts = fileName.Replace(DEFAULT_IMG_EXTENSION, "", StringComparison.OrdinalIgnoreCase).Split('_');

        if (parts.Length < 4)
        {
            originalFileName = string.Empty;
            pathParts = [];
            return false;
        }

        string origin = parts[1];
        string id = parts[2];
        string folder = parts[3];

        originalFileName = $"{id}{DEFAULT_IMG_EXTENSION}";
        pathParts = [IMAGE_FOLDER_NAME, origin, folder];

        return true;
    }

    public string[] GetPathParts(string relativePath)
    {
        return Path.GetDirectoryName(relativePath)?
                   .Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                   .Where(p => !string.IsNullOrEmpty(p))
                   .ToArray() ?? [];
    }

    public string FormatPath(string[] parts)
    {
        return string.Join(Path.DirectorySeparatorChar, parts);
    }

    public void DeleteFile(string fullPath)
    {
        if (File.Exists(fullPath)) File.Delete(fullPath);
    }

    private static string GetNormalizedPath(string relativePath)
    {
        return relativePath.Replace(Path.DirectorySeparatorChar, '/').TrimStart('/');
    }

    private static void CheckDirectory(string path)
    {
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
    }

    private static string GetLocalStaticPath(string path)
    {
        if (Path.DirectorySeparatorChar == '/')
        {
            return path;
        }
        return path.Replace("/host_mnt", "").Replace("/c/", "C:\\").Replace("/", "\\");
    }
}
