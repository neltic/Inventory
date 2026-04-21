using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Stock.Api.Utils;
using Stock.Application.Common;
using Stock.Infrastructure.Persistence;
using System.Text;

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

// Register TokenOptions configuration 
builder.Services.Configure<TokenOptions>(
    builder.Configuration.GetSection(TokenOptions.SectionName));

var tokenOptions = builder.Configuration
    .GetSection(TokenOptions.SectionName)
    .Get<TokenOptions>();

// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions!.SecretKey)),
            ValidateIssuer = true,
            ValidIssuer = tokenOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = tokenOptions.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

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

builder.Services.AddAuthorization();

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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();