namespace Stock.Worker;

public partial class CloudBackupWorker
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Active Image Watcher: {Path}")]
    static partial void LogWatcherActive(ILogger logger, string path);

    [LoggerMessage(Level = LogLevel.Information, Message = "Starting sync local to cloud")]
    static partial void LogInitialSyncStart(ILogger logger);

    [LoggerMessage(Level = LogLevel.Information, Message = "Initial sync completed. {Count} files verified.")]
    static partial void LogInitialSyncEnd(ILogger logger, int count);

    [LoggerMessage(Level = LogLevel.Information, Message = "{Action}: {File}")]
    static partial void LogSyncSuccess(ILogger logger, string action, string file);

    [LoggerMessage(Level = LogLevel.Information, Message = "Cleaned cache: {Count} freed routes.")]
    static partial void LogCacheCleanup(ILogger logger, int count);

    [LoggerMessage(Level = LogLevel.Information, Message = "Skipping {File}: already synchronized")]
    static partial void LogSkipFile(ILogger logger, string file);

    [LoggerMessage(Level = LogLevel.Error, Message = "Failed to upload: {File}")]
    static partial void LogSyncError(ILogger logger, string file, Exception? exception);

    [LoggerMessage(Level = LogLevel.Error, Message = "Initial sync error: {Message}")]
    static partial void LogInitialSyncError(ILogger logger, string message);

    [LoggerMessage(Level = LogLevel.Information, Message = "Remote file sent to trash: {File}")]
    static partial void LogRemoteDelete(ILogger logger, string file);
}