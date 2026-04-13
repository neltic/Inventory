using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Microsoft.AspNetCore.StaticFiles;
using System.Collections.Concurrent;

namespace Stock.Worker;

public class CloudBackupWorker : BackgroundService
{
    private readonly ILogger<CloudBackupWorker> _logger;
    private readonly string _imgFolderId;
    private readonly string _localImgPath;
    private DriveService _driveService;
    private readonly ConcurrentDictionary<string, string> _folderCache = new();
    private readonly ConcurrentDictionary<string, DateTime> _processedFiles = new();
    private static readonly SemaphoreSlim _folderLock = new SemaphoreSlim(1, 1);
    private const int WAIT_TO_PROCESS = 3000; // mili seconds

    public CloudBackupWorker(ILogger<CloudBackupWorker> logger)
    {
        DotNetEnv.Env.TraversePath().Load();
        _logger = logger;
        _imgFolderId = Environment.GetEnvironmentVariable("GOOGLE_DRIVE_IMAGE_FOLDER_ID") ?? "";
        _localImgPath = Environment.GetEnvironmentVariable("LOCAL_IMAGE_STORAGE_PATH") ?? "";
        _driveService = InitializeDriveService();
    }

    private DriveService InitializeDriveService()
    {
        string clientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID") ?? "";
        string clientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET") ?? "";
        string refreshToken = Environment.GetEnvironmentVariable("GOOGLE_REFRESH_TOKEN") ?? "";
        string userId = Environment.GetEnvironmentVariable("WORKER_USER_ID") ?? "";
        string appName = Environment.GetEnvironmentVariable("WORKER_APP_NAME") ?? "";
        var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            }
        });
        var tokenResponse = new TokenResponse { RefreshToken = refreshToken };
        var credential = new UserCredential(flow, userId, tokenResponse);

        return new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = appName
        });
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_logger.IsEnabled(LogLevel.Information))
            _logger.LogInformation("Active Image Watcher: {path}", _localImgPath);

        if (!Directory.Exists(_localImgPath)) Directory.CreateDirectory(_localImgPath);

        using var watcher = new FileSystemWatcher(_localImgPath)
        {
            IncludeSubdirectories = true,
            EnableRaisingEvents = true
        };

        watcher.Created += (s, e) => _ = Task.Run(() => ProcessFile(e.FullPath, e.Name));
        watcher.Changed += (s, e) => _ = Task.Run(() => ProcessFile(e.FullPath, e.Name));

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task<string> GetOrCreateFolderRecursive(string[] pathParts)
    {
        string currentParentId = _imgFolderId;
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
            finally
            {
                _folderLock.Release();
            }
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
            if (_logger.IsEnabled(LogLevel.Information))
                _logger.LogInformation("Creating remote folder: {name}", folderName);
            var folderMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = folderName,
                MimeType = "application/vnd.google-apps.folder",
                Parents = new List<string> { parentId }
            };
            var newFolder = await _driveService.Files.Create(folderMetadata).ExecuteAsync();
            folderId = newFolder.Id;
        }

        return folderId;
    }

    private async Task ProcessFile(string fullPath, string? relativePath)
    {
        if (string.IsNullOrEmpty(relativePath) || Directory.Exists(fullPath)) return;

        var now = DateTime.UtcNow;
        if (_processedFiles.TryGetValue(fullPath, out var lastProcessed))
        {
            if ((now - lastProcessed).TotalSeconds < (WAIT_TO_PROCESS / 1000.0)) return;
        }
        _processedFiles[fullPath] = now;

        var pathParts = Path.GetDirectoryName(relativePath)?
                            .Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                            .Where(p => !string.IsNullOrEmpty(p))
                            .ToArray() ?? Array.Empty<string>();

        await UploadToDrive(fullPath, Path.GetFileName(relativePath), pathParts);
    }

    private async Task UploadToDrive(string fullPath, string fileName, string[] pathParts)
    {
        try
        {
            await Task.Delay(WAIT_TO_PROCESS);
            if (!File.Exists(fullPath)) return;

            string targetFolderId = await GetOrCreateFolderRecursive(pathParts);
            string mimeType = GetMimeTypeBy(fullPath);

            var listRequest = _driveService.Files.List();
            listRequest.Q = $"name = '{fileName}' and '{targetFolderId}' in parents and trashed = false";
            listRequest.Fields = "files(id)";
            var existingFiles = await listRequest.ExecuteAsync();
            var existingFile = existingFiles.Files.FirstOrDefault();

            using var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

            if (existingFile != null)
            {
                var updateRequest = _driveService.Files.Update(new Google.Apis.Drive.v3.Data.File(), existingFile.Id, stream, mimeType);
                ProcessResult(await updateRequest.UploadAsync(), "update", fileName, pathParts);
            }
            else
            {                
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = fileName,
                    Parents = new List<string> { targetFolderId }
                };
                var createRequest = _driveService.Files.Create(fileMetadata, stream, mimeType);
                ProcessResult(await createRequest.UploadAsync(), "create", fileName, pathParts);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Sync Error: {exception}", ex);
        }
    }

    private void ProcessResult(IUploadProgress result, string action, string fileName, string[] pathParts)
    {
        if (result.Status == UploadStatus.Completed)
        {
            if (_logger.IsEnabled(LogLevel.Information))
                _logger.LogInformation("Synchronized ({action}): {path}/{file}", action, string.Join("/", pathParts), fileName);
        }
        else
            _logger.LogError("Failed to upload: {path}/{file} - {exception}", string.Join("/", pathParts), fileName, result.Exception);
    }

    private string GetMimeTypeBy(string fullPath)
    {
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(fullPath, out string? mimeType))
        {
            mimeType = "application/octet-stream";
        }
        return mimeType;
    }
}