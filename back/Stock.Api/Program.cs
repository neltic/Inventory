using Microsoft.EntityFrameworkCore;
using Stock.Api.Utils;
using Stock.Application.Common;
using Stock.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// CORS service
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificAngularOrigins",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
});

builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<StockDbContext>(options =>
    options.UseSqlServer(connectionString));

// IoC registration
builder.Services.AddProjectDependencies();

// Register FileStorageOptions configuration
builder.Services.Configure<FileStorageOptions>(
    builder.Configuration.GetSection(FileStorageOptions.SectionName));

// Build application
var app = builder.Build();

// Run database update on startup
app.Services.RunDataBaseUpdate();

#if DEBUG
// Dummy Data
await app.Services.CreateDummyDataAsync();
#endif

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("AllowSpecificAngularOrigins");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();