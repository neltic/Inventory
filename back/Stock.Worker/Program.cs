using Stock.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<CloudBackupWorker>();

var host = builder.Build();
host.Run();
