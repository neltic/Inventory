using Stock.Foundation.Common;
using Stock.Worker.Common;
using Stock.Worker.Interfaces;

namespace Stock.Worker.Services;

public class LocalFileService(LocalFileOptions options) : ILocalFileService
{
    private readonly string StaticPath = FileRegistry.Path.GetLocal(options.StoragePath);

    public string GetStaticPath() => StaticPath;

    public string GetImagePath() => Path.Combine(StaticPath, FileRegistry.Folder.Image);

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
        var parts = syncFileName.Replace(FileRegistry.Extension.Sync, "", StringComparison.OrdinalIgnoreCase).Split('_');

        if (parts is not [_, var origin, var id, ..])
        {
            pendingFiles = [];
            return false;
        }

        pendingFiles = [.. FileRegistry.Folder.SubFolder.All.Select(sub => Path.Combine(GetImagePath(), origin, sub, FileRegistry.GetImageName(id)))];

        return true;
    }

    public string GetRelativeToStaticPath(string file)
    {
        return Path.GetRelativePath(StaticPath, file);
    }

    public bool IsRemovable(string relativePath, out string fileName)
    {
        fileName = Path.GetFileName(relativePath);
        string normalizedPath = FileRegistry.Path.Normalize(relativePath);
        return normalizedPath.StartsWith(FileRegistry.Folder.TempSlashed, StringComparison.OrdinalIgnoreCase) &&
               fileName.StartsWith(FileRegistry.Prefix.Deleted, StringComparison.OrdinalIgnoreCase);
    }

    public bool IsInImageFolder(string relativePath)
    {
        string normalizedPath = FileRegistry.Path.Normalize(relativePath);
        return normalizedPath.StartsWith(FileRegistry.Folder.ImageSlashed, StringComparison.OrdinalIgnoreCase);
    }

    public bool IsSyncFile(string relativePath, out string fileName)
    {
        fileName = Path.GetFileName(relativePath);
        string normalizedPath = FileRegistry.Path.Normalize(relativePath);
        return normalizedPath.StartsWith(FileRegistry.Folder.TempSlashed, StringComparison.OrdinalIgnoreCase) &&
               fileName.StartsWith(FileRegistry.Prefix.Sync, StringComparison.OrdinalIgnoreCase) &&
               fileName.EndsWith(FileRegistry.Extension.Sync, StringComparison.OrdinalIgnoreCase);
    }

    public bool IsDbBackupFile(string relativePath, out string fileName)
    {
        fileName = Path.GetFileName(relativePath);
        string normalizedPath = FileRegistry.Path.Normalize(relativePath);
        return normalizedPath.StartsWith(FileRegistry.Folder.TempSlashed, StringComparison.OrdinalIgnoreCase) &&
               fileName.StartsWith(FileRegistry.Prefix.DbBackup, StringComparison.OrdinalIgnoreCase) &&
               fileName.EndsWith(FileRegistry.Extension.DbBackup, StringComparison.OrdinalIgnoreCase);
    }

    public bool TryGetFileNameAndParts(string fileName, out string originalFileName, out string[] pathParts)
    {
        var parts = fileName.Replace(FileRegistry.Extension.Image, "", StringComparison.OrdinalIgnoreCase).Split('_');

        if (parts is not [_, var origin, var id, var folder, ..])
        {
            originalFileName = string.Empty;
            pathParts = [];
            return false;
        }

        originalFileName = FileRegistry.GetImageName(id);
        pathParts = [FileRegistry.Folder.Image, origin, folder];

        return true;
    }

    public string[] GetPathParts(string relativePath)
    {
        return Path.GetDirectoryName(relativePath)?
                   .Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                   .Where(p => !string.IsNullOrEmpty(p))
                   .ToArray() ?? [];
    }

    public string[] GetDbPathParts()
    {
        return [FileRegistry.Folder.Database];
    }

    public string FormatPath(string[] parts)
    {
        return string.Join(Path.DirectorySeparatorChar, parts);
    }

    public void DeleteFile(string fullPath)
    {
        if (File.Exists(fullPath)) File.Delete(fullPath);
    }

    private static void CheckDirectory(string path)
    {
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
    }
}
