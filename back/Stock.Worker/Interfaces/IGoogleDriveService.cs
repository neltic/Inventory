using Google.Apis.Upload;

namespace Stock.Worker.Interfaces;

public interface IGoogleDriveService
{
    Task<string> GetOrCreateFolderRecursiveAsync(string[] pathParts);

    Task<Google.Apis.Drive.v3.Data.File?> GetExistingFileAsync(string fileName, string folderId);

    Task<IUploadProgress> UploadFileAsync(string fullPath, string fileName, string folderId, string? existingFileId = null);

    Task MoveToTrashAsync(string fileId);

    bool IsAlreadySync(Google.Apis.Drive.v3.Data.File? driveFile, string localPath);
}
