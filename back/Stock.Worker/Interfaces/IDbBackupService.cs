namespace Stock.Worker.Interfaces;

public interface IDbBackupService
{
    Task<string> CreateAsync();

    Task<DateTime> GetLastBackupDateAsync();
}