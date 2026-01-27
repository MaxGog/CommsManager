using Microsoft.Extensions.DependencyInjection;

namespace CommsManager.Maui.Services;

public static class DatabaseFactory
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services)
    {
        services.AddSingleton<DatabaseService>();
        services.AddSingleton<IDatabaseInitializer, DatabaseInitializer>();
        services.AddSingleton<IDatabaseMigrator, DatabaseMigrator>();
        services.AddSingleton<IDatabaseBackupService, DatabaseBackupService>();

        return services;
    }
}

public interface IDatabaseInitializer
{
    Task InitializeAsync();
    Task<bool> NeedsMigrationAsync();
}

public class DatabaseInitializer : IDatabaseInitializer
{
    private readonly DatabaseService _databaseService;

    public DatabaseInitializer(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task InitializeAsync()
    {
        await _databaseService.GetCustomersAsync();
    }

    public async Task<bool> NeedsMigrationAsync()
    {
        return false;
    }
}

public interface IDatabaseMigrator
{
    Task MigrateAsync(int fromVersion, int toVersion);
    Task<int> GetCurrentVersionAsync();
}

public class DatabaseMigrator : IDatabaseMigrator
{
    private readonly DatabaseService _databaseService;

    public DatabaseMigrator(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<int> GetCurrentVersionAsync()
    {
        var result = await _databaseService.QueryAsync<VersionInfo>(
            "SELECT * FROM VersionInfo ORDER BY Version DESC LIMIT 1");

        return result.FirstOrDefault()?.Version ?? 0;
    }

    public async Task MigrateAsync(int fromVersion, int toVersion)
    {
        for (int version = fromVersion + 1; version <= toVersion; version++)
        {
            await ApplyMigrationAsync(version);
        }
    }

    private async Task ApplyMigrationAsync(int version)
    {
        switch (version)
        {
            case 1:
                await _databaseService.ExecuteQueryAsync(
                    "CREATE TABLE IF NOT EXISTS VersionInfo (Version INTEGER PRIMARY KEY, AppliedOn DATETIME)");
                await _databaseService.ExecuteQueryAsync(
                    "INSERT INTO VersionInfo (Version, AppliedOn) VALUES (1, ?)", DateTime.UtcNow);
                break;

            case 2:
                await _databaseService.ExecuteQueryAsync(
                    "ALTER TABLE Customers ADD COLUMN Notes TEXT");
                await _databaseService.ExecuteQueryAsync(
                    "INSERT INTO VersionInfo (Version, AppliedOn) VALUES (2, ?)", DateTime.UtcNow);
                break;
        }
    }
}

public class VersionInfo
{
    public int Version { get; set; }
    public DateTime AppliedOn { get; set; }
}

public interface IDatabaseBackupService
{
    Task<string> CreateBackupAsync();
    Task<bool> RestoreFromBackupAsync(string backupPath);
    Task<List<string>> GetBackupsAsync();
    Task<bool> DeleteBackupAsync(string backupPath);
}

public class DatabaseBackupService : IDatabaseBackupService
{
    private readonly DatabaseService _databaseService;

    public DatabaseBackupService(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public async Task<string> CreateBackupAsync()
    {
        var backupDir = Path.Combine(FileSystem.AppDataDirectory, "backups");
        Directory.CreateDirectory(backupDir);

        var backupPath = Path.Combine(backupDir, $"backup_{DateTime.Now:yyyyMMdd_HHmmss}.db3");

        await _databaseService.BackupDatabaseAsync(backupPath);
        return backupPath;
    }

    public async Task<bool> RestoreFromBackupAsync(string backupPath)
    {
        if (!File.Exists(backupPath))
            return false;

        try
        {
            await _databaseService.DisposeAsync();

            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "commsmanager.db3");
            File.Copy(backupPath, dbPath, true);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public Task<List<string>> GetBackupsAsync()
    {
        var backupDir = Path.Combine(FileSystem.AppDataDirectory, "backups");

        if (!Directory.Exists(backupDir))
            return Task.FromResult(new List<string>());

        var backups = Directory.GetFiles(backupDir, "*.db3")
            .OrderByDescending(f => f)
            .ToList();

        return Task.FromResult(backups);
    }

    public Task<bool> DeleteBackupAsync(string backupPath)
    {
        try
        {
            File.Delete(backupPath);
            return Task.FromResult(true);
        }
        catch
        {
            return Task.FromResult(false);
        }
    }
}