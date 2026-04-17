using Microsoft.EntityFrameworkCore;
using Stock.Api.Utils;
using Stock.Application.Common;
using Stock.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
}

var redisConnectionHost = builder.Configuration.GetValue<string>("RedisConfig:ConnectionHost");

if (string.IsNullOrWhiteSpace(redisConnectionHost))
{
    throw new InvalidOperationException("Connection 'RedisConfig:ConnectionHost' is not configured.");
}

builder.Services.AddDbContext<StockDbContext>(options =>
    options.UseSqlServer(connectionString));

// IoC registration
builder.Services.AddProjectDependencies();

// Register singleton services
builder.Services.AddSingletonDependencies();

// Register FileStorageOptions configuration
builder.Services.Configure<FileStorageOptions>(
    builder.Configuration.GetSection(FileStorageOptions.SectionName));

builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = $"{redisConnectionHost},abortConnect=false,connectTimeout=5000");

// Add health checks
builder.Services.AddHealthChecks()
    .AddSqlServer(connectionString: connectionString,
        name: "sql-check",
        tags: ["db", "sql"]);

// Build application
var app = builder.Build();

// Run database update on startup
app.Services.RunDataBaseUpdate();

#if DEBUG
// Dummy Data
await app.Services.CreateDummyDataAsync();
#endif

// Initialize cache on startup
await app.Services.InitializeCacheAsync();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<LanguageDetectionMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();