using Stock.Worker;
using Stock.Worker.Common;
using Stock.Worker.Interfaces;
using Stock.Worker.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        DotNetEnv.Env.TraversePath().Load();

        services.AddSingleton(new GoogleDriveOptions
        {
            ClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID") ?? "",
            ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET") ?? "",
            RefreshToken = Environment.GetEnvironmentVariable("GOOGLE_REFRESH_TOKEN") ?? "",
            UserId = Environment.GetEnvironmentVariable("WORKER_USER_ID") ?? "",
            AppName = Environment.GetEnvironmentVariable("WORKER_APP_NAME") ?? "",
            RootFolderId = Environment.GetEnvironmentVariable("GOOGLE_DRIVE_FOLDER_ID") ?? ""
        });

        services.AddSingleton<IGoogleDriveService, GoogleDriveService>();

        services.AddSingleton(new LocalFileOptions
        {
            StoragePath = Environment.GetEnvironmentVariable("STATIC_STORAGE_PATH") ?? ""
        });

        services.AddSingleton<ILocalFileService, LocalFileService>();

        services.AddSingleton<IProcessedFileService, ProcessedFileService>();

        services.AddSingleton(new DbBackupOptions
        {
            ConnectionString = Environment.GetEnvironmentVariable("DB_BACKUP_CONNECTION") ?? "",
            Path = Environment.GetEnvironmentVariable("DB_BACKUP_PATH") ?? "",
            StoragePath = Environment.GetEnvironmentVariable("STATIC_STORAGE_PATH") ?? ""
        });

        services.AddSingleton<IDbBackupService, DbBackupService>();

        services.AddHostedService<CloudBackupWorker>();
    })
    .Build();

await host.RunAsync();