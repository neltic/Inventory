using Google;
using Google.Apis.Upload;
using Stock.Worker.Interfaces;

namespace Stock.Worker;

public partial class CloudBackupWorker(
    ILogger<CloudBackupWorker> logger,
    IGoogleDriveService driveService,
    ILocalFileService localFileService,
    IProcessedFileService processedFileService
    ) : BackgroundService
{
    private const int WAIT_TO_PROCESS_MS = 3000;
    private const int INITIAL_SYNC_DELAY_MS = 300;
    private const int CLEANUP_INTERVAL_MIN = 15;
    private const int QUICK_CHECK_DELAY_MS = 300;
    private const int MAX_RETRIES = 3;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        string storagePath = localFileService.GetStaticPath();
        LogWatcherActive(logger, storagePath);

        localFileService.CheckStaticDirectory();

        using var watcher = new FileSystemWatcher(storagePath)
        {
            IncludeSubdirectories = true,
            EnableRaisingEvents = true,
            InternalBufferSize = 65536
        };
                
        watcher.Created += (s, e) => _ = Task.Run(() => ProcessFile(e.FullPath, e.Name));
        watcher.Changed += (s, e) => _ = Task.Run(() => ProcessFile(e.FullPath, e.Name));
        
        _ = Task.Run(() => SyncLocalToCloud(stoppingToken), stoppingToken);

        DateTime lastCleanup = DateTime.UtcNow;

        while (!stoppingToken.IsCancellationRequested)
        {
            if ((DateTime.UtcNow - lastCleanup).TotalMinutes >= CLEANUP_INTERVAL_MIN)
            {
                int cleaned = processedFileService.CleanupProcessedFiles();
                if (cleaned > 0) LogCacheCleanup(logger, cleaned);
                lastCleanup = DateTime.UtcNow;
            }

            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task SyncLocalToCloud(CancellationToken ct)
    {
        try
        {
            LogInitialSyncStart(logger);

            var files = localFileService.GetFiles();

            foreach (var file in files)
            {
                if (ct.IsCancellationRequested) break;

                string relativePath = localFileService.GetRelativeToStaticPath(file);

                await ProcessFile(file, relativePath);

                await Task.Delay(INITIAL_SYNC_DELAY_MS, ct);
            }

            LogInitialSyncEnd(logger, files.Length);
        }
        catch (Exception ex)
        {
            LogInitialSyncError(logger, ex.Message);
        }
    }

    private async Task ProcessFile(string fullPath, string? relativePath)
    {
        if (string.IsNullOrEmpty(relativePath) || Directory.Exists(fullPath)) return;
        
        if (localFileService.IsRemovable(relativePath, out var fileName))
        {
            await HandleDeletionFromTemp(fullPath, fileName);
            return;
        }

        if (localFileService.IsInImageFolder(relativePath))
        {
            if (!processedFileService.ShouldProcess(fullPath, WAIT_TO_PROCESS_MS)) return;

            var pathParts = localFileService.GetPathParts(relativePath);

            await UploadToDrive(fullPath, Path.GetFileName(relativePath), pathParts);
            return;
        }
        
        if (localFileService.IsSyncFile(relativePath, out var syncFileName))
        {
            await HandleSyncFromTemp(fullPath, syncFileName);
            return;
        }
    }

    private async Task UploadToDrive(string fullPath, string fileName, string[] pathParts)
    {
        int retryCount = 0;

        while (retryCount < MAX_RETRIES)
        {
            try
            {
                await Task.Delay(QUICK_CHECK_DELAY_MS);

                if (!File.Exists(fullPath)) return;

                string folderId = await driveService.GetOrCreateFolderRecursiveAsync(pathParts);
                var existingFile = await driveService.GetExistingFileAsync(fileName, folderId);

                if (driveService.IsAlreadySync(existingFile, fullPath))
                {
                    LogSkipFile(logger, fileName);
                    return;
                }

                await Task.Delay(WAIT_TO_PROCESS_MS - QUICK_CHECK_DELAY_MS);

                if (!File.Exists(fullPath)) return;

                var result = await driveService.UploadFileAsync(fullPath, fileName, folderId, existingFile?.Id);

                ProcessResult(result, existingFile == null ? "Created" : "Updated", fileName, pathParts);

                break;
            }
            catch (GoogleApiException ex) when (
                ex.HttpStatusCode == System.Net.HttpStatusCode.ServiceUnavailable ||
                ex.HttpStatusCode == System.Net.HttpStatusCode.Gone)
            {
                retryCount++;
                logger.LogWarning("Google temporary error. Retry {n}/{max} for {file}", retryCount, MAX_RETRIES, fileName);
                await Task.Delay(WAIT_TO_PROCESS_MS * retryCount);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Sync Error");
                break;
            }
        }
    }


    private async Task HandleSyncFromTemp(string fullPath, string syncFileName)
    {
        if (!File.Exists(fullPath)) return;

        localFileService.DeleteFile(fullPath);

        if (localFileService.TryGetPendingSyncFiles(syncFileName, out var pendingFiles))
        {            
            foreach (var pendingFile in pendingFiles)
            {
                await ProcessFile(pendingFile, localFileService.GetRelativeToStaticPath(pendingFile));
            }
        }
    }

    private async Task HandleDeletionFromTemp(string fullPath, string fileName)
    {
        if (!processedFileService.ShouldProcess(fullPath, WAIT_TO_PROCESS_MS)) return;

        if (!File.Exists(fullPath)) return;

        try
        {
            if (!localFileService.TryGetFileNameAndParts(fileName, out string originalFileName, out string[] pathParts)) return;

            string folderId = await driveService.GetOrCreateFolderRecursiveAsync(pathParts);
            var driveFile = await driveService.GetExistingFileAsync(originalFileName, folderId);

            localFileService.DeleteFile(fullPath);

            if (driveFile != null)
            {
                string displayPath = localFileService.FormatPath([.. pathParts, originalFileName]);
                await driveService.MoveToTrashAsync(driveFile.Id);
                LogRemoteDelete(logger, displayPath);
            }
        }
        catch (Exception ex) { logger.LogError(ex, "Error during remote deletion"); }
    }

    private void ProcessResult(IUploadProgress result, string action, string fileName, string[] pathParts)
    {
        string displayPath = localFileService.FormatPath([.. pathParts, fileName]);

        if (result.Status == UploadStatus.Completed)
            LogSyncSuccess(logger, action, displayPath);
        else
            LogSyncError(logger, displayPath, result.Exception);
    }
}