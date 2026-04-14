namespace Stock.Worker.Interfaces;

public interface ILocalFileService
{
    string GetStaticPath();
    string GetImagePath();
    void CheckStaticDirectory();
    string[] GetFiles();
    bool TryGetPendingSyncFiles(string syncFileName, out string[] pendingFiles);
    string GetRelativeToStaticPath(string file);
    bool IsRemovable(string relativePath, out string fileName);
    bool IsInImageFolder(string relativePath);
    bool IsSyncFile(string relativePath, out string fileName);
    bool TryGetFileNameAndParts(string fileName, out string originalFileName, out string[] pathParts);
    string[] GetPathParts(string relativePath);
    string FormatPath(string[] parts);
    void DeleteFile(string fullPath);
}
