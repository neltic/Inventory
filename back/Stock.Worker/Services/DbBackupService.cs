using Microsoft.Data.SqlClient;
using Stock.Foundation.Common;
using Stock.Worker.Common;
using Stock.Worker.Interfaces;

namespace Stock.Worker.Services;

public class DbBackupService(DbBackupOptions options) : IDbBackupService
{
    public async Task<string> CreateAsync()
    {
        using var connection = new SqlConnection(options.ConnectionString);

        await connection.OpenAsync();

        string dbName = connection.Database;
        string fileName = FileRegistry.GetDbBackupFileName(dbName);
        string fullPath = Path.Combine(FileRegistry.Path.GetLocal(options.Path), fileName);
        string query = $"BACKUP DATABASE [{dbName}] TO DISK = @path WITH FORMAT, MEDIANAME = 'SQLServerBackups', NAME = 'Full Backup of {dbName}';";

        using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@path", fullPath);
        command.CommandTimeout = 300;

        await command.ExecuteNonQueryAsync();

        return fileName;
    }

    public async Task<DateTime> GetLastBackupDateAsync()
    {
        var directoryPath = Path.Combine(FileRegistry.Path.GetLocal(options.StoragePath), FileRegistry.Folder.Temp);

        if (!Directory.Exists(directoryPath)) return DateTime.MinValue;

        var directory = new DirectoryInfo(directoryPath);

        var lastFile = directory.GetFiles($"{FileRegistry.Prefix.DbBackup}*{FileRegistry.Extension.DbBackup}")
                                .OrderByDescending(f => f.CreationTimeUtc)
                                .FirstOrDefault();

        return lastFile?.CreationTimeUtc ?? DateTime.MinValue;
    }
}
