using Stock.Worker.Interfaces;
using System.Collections.Concurrent;

namespace Stock.Worker.Services;

public class ProcessedFileService : IProcessedFileService
{
    private const int CLEAN_PROCESSED_FILES_INTERVAL_MIN = 5;
    private readonly ConcurrentDictionary<string, DateTime> _processedFiles = new();

    public bool ShouldProcess(string fullPath, int waitTimeMs)
    {
        var now = DateTime.UtcNow;
        bool shouldProcess = false;

        _processedFiles.AddOrUpdate(
            fullPath,            
            (key) => {
                shouldProcess = true;
                return now;
            },            
            (key, lastProcessed) => {
                if ((now - lastProcessed).TotalMilliseconds >= waitTimeMs)
                {
                    shouldProcess = true;
                    return now;
                }
                shouldProcess = false;
                return lastProcessed;
            }
        );

        return shouldProcess;
    }

    public int CleanupProcessedFiles()
    {
        var now = DateTime.UtcNow;
        var expirationThreshold = TimeSpan.FromMinutes(CLEAN_PROCESSED_FILES_INTERVAL_MIN);
        int removedCount = 0;

        foreach (var kvp in _processedFiles)
        {
            if ((now - kvp.Value) > expirationThreshold)
            {
                if (_processedFiles.TryRemove(kvp.Key, out _))
                {
                    removedCount++;
                }
            }
        }
        return removedCount;
    }
}
