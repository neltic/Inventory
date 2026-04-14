using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Worker.Interfaces;

public interface IProcessedFileService
{
    bool ShouldProcess(string fullPath, int waitTimeMs);
    int CleanupProcessedFiles();
}
