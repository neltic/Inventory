using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Microsoft.AspNetCore.StaticFiles;
using Stock.Worker.Common;
using Stock.Worker.Interfaces;
using System.Collections.Concurrent;

namespace Stock.Worker.Services;

public class GoogleDriveService(GoogleDriveOptions options) : IGoogleDriveService
{
    private readonly DriveService _driveService = InitializeService(options);
    private readonly string _rootFolderId = options.RootFolderId;
    private readonly ConcurrentDictionary<string, string> _folderCache = new();
    private static readonly SemaphoreSlim _folderLock = new(1, 1);
    private const int TIME_PRECISION_TOLERANCE_SEC = 2;

    public async Task<string> GetOrCreateFolderRecursiveAsync(string[] pathParts)
    {
        string currentParentId = _rootFolderId;
        string currentPathKey = "root";

        foreach (var part in pathParts)
        {
            currentPathKey = $"{currentPathKey}/{part.ToLower()}";
            if (_folderCache.TryGetValue(currentPathKey, out var cachedId))
            {
                currentParentId = cachedId;
                continue;
            }

            await _folderLock.WaitAsync();

            try
            {
                if (_folderCache.TryGetValue(currentPathKey, out var retryCachedId))
                {
                    currentParentId = retryCachedId;
                }
                else
                {
                    currentParentId = await GetOrCreateSingleFolder(part, currentParentId);
                    _folderCache.TryAdd(currentPathKey, currentParentId);
                }
            }
            finally { _folderLock.Release(); }
        }
        return currentParentId;
    }

    private async Task<string> GetOrCreateSingleFolder(string folderName, string parentId)
    {
        var listRequest = _driveService.Files.List();
        listRequest.Q = $"name = '{folderName}' and '{parentId}' in parents and mimeType = 'application/vnd.google-apps.folder' and trashed = false";
        listRequest.Fields = "files(id)";

        var result = await listRequest.ExecuteAsync();
        var folderId = result.Files.FirstOrDefault()?.Id;

        if (folderId == null)
        {
            var folderMetadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = folderName,
                MimeType = "application/vnd.google-apps.folder",
                Parents = [parentId]
            };
            var newFolder = await _driveService.Files.Create(folderMetadata).ExecuteAsync();
            folderId = newFolder.Id;
        }
        return folderId;
    }

    public async Task<Google.Apis.Drive.v3.Data.File?> GetExistingFileAsync(string fileName, string folderId)
    {
        var listRequest = _driveService.Files.List();
        listRequest.Q = $"name = '{fileName}' and '{folderId}' in parents and trashed = false";
        listRequest.Fields = "files(id, modifiedTime)";
        var result = await listRequest.ExecuteAsync();
        return result.Files.FirstOrDefault();
    }

    public async Task<IUploadProgress> UploadFileAsync(string fullPath, string fileName, string folderId, string? existingFileId = null)
    {
        var mimeType = GetMimeType(fullPath);
        using var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

        var fileMetadata = new Google.Apis.Drive.v3.Data.File { ModifiedTimeDateTimeOffset = File.GetLastWriteTimeUtc(fullPath) };

        if (existingFileId != null)
        {
            var updateRequest = _driveService.Files.Update(fileMetadata, existingFileId, stream, mimeType);
            return await updateRequest.UploadAsync();
        }

        fileMetadata.Name = fileName;
        fileMetadata.Parents = [folderId];
        var createRequest = _driveService.Files.Create(fileMetadata, stream, mimeType);
        return await createRequest.UploadAsync();
    }

    public async Task MoveToTrashAsync(string fileId)
    {
        await _driveService.Files.Update(new Google.Apis.Drive.v3.Data.File { Trashed = true }, fileId).ExecuteAsync();
    }

    public bool IsAlreadySync(Google.Apis.Drive.v3.Data.File? driveFile, string localPath)
    {
        if (driveFile?.ModifiedTimeDateTimeOffset == null) return false;
        var diff = Math.Abs((driveFile.ModifiedTimeDateTimeOffset.Value - File.GetLastWriteTimeUtc(localPath)).TotalSeconds);
        return diff <= TIME_PRECISION_TOLERANCE_SEC;
    }

    private static DriveService InitializeService(GoogleDriveOptions options)
    {
        var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = options.ClientId,
                ClientSecret = options.ClientSecret
            }
        });

        var tokenResponse = new TokenResponse { RefreshToken = options.RefreshToken };
        var credential = new UserCredential(flow, options.UserId, tokenResponse);

        return new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = options.AppName
        });
    }

    private static string GetMimeType(string path)
    {
        new FileExtensionContentTypeProvider().TryGetContentType(path, out string? mimeType);
        return mimeType ?? "application/octet-stream";
    }
}