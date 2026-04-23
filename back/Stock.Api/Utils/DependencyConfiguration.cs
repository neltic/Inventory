using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Stock.Application.Common;
using Stock.Application.Interfaces;
using Stock.Application.Interfaces.Common;
using Stock.Domain.Entities;
using Stock.Infrastructure.Persistence;
using Stock.Infrastructure.Services;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;

namespace Stock.Api.Utils;

public static class DependencyConfiguration
{
    private enum MessageSource
    {
        DependencyInjection,
        DataBase,
        Globalization,
        Error,
        None
    }

    public static void AddScopedDependencies(this IServiceCollection services)
    {
        var assemblies = new[] { "Stock.Application", "Stock.Infrastructure" };

        foreach (var assemblyName in assemblies)
        {
            InfoMessage(MessageSource.DependencyInjection, $"{assemblyName}:");

            var assembly = Assembly.Load(assemblyName);

            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract &&
                           (t.Name.EndsWith("Service") || t.Name.EndsWith("Repository")));

            foreach (var type in types)
            {
                var interfaceType = type.GetInterface($"I{type.Name}");
                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, type);
                    OkMessage(MessageSource.None, $"{interfaceType.Name} -> {type.Name}");
                }
            }
        }
        Console.ResetColor();
    }

    public static void AddSingletonDependencies(this IServiceCollection services)
    {
        InfoMessage(MessageSource.DependencyInjection, $"Singleton injected:");

        services.AddSingleton<ITranslationStorage, TranslationStorage>();
        OkMessage(MessageSource.None, $"ITranslationStorage -> TranslationStorage");

        Console.ResetColor();
    }

    public static void RunDataBaseUpdate(this IServiceProvider appServices)
    {
        using var scope = appServices.CreateScope();

        var serviceProvider = scope.ServiceProvider;

        try
        {
            InfoMessage(MessageSource.DataBase, "Starting database update...");

            var context = serviceProvider.GetRequiredService<StockDbContext>();

            context.Database.Migrate();

            OkMessage(MessageSource.DataBase, "Database and stored procedures successfully updated!");
        }
        catch (Exception ex)
        {
            ErrorMessage(MessageSource.DataBase, "CRITICAL ERROR when adding database data:");
            ErrorMessage(MessageSource.Error, ex.Message);
        }
        finally
        {
            Console.ResetColor();
        }
    }

    public static void ConfigureAuth(this IServiceCollection services, KeycloakOptions keycloakOptions)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.Authority = keycloakOptions.Authority;
            options.Audience = keycloakOptions.Audience;            
            options.MetadataAddress = keycloakOptions.MetadataAddress;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,                
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(keycloakOptions.ClockSkewSeconds),
                RoleClaimType = ClaimTypes.Role
            };
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    var principal = context.Principal;
                    if (principal == null)
                    {
                        return Task.CompletedTask;
                    }
                    var rAccess = principal.FindFirst("realm_access");
                    if (rAccess != null && !string.IsNullOrEmpty(rAccess.Value))
                    {
                        try
                        {
                            using var payload = JsonDocument.Parse(rAccess.Value);
                            if (payload.RootElement.TryGetProperty("roles", out var roles))
                            {
                                if (principal.Identity is ClaimsIdentity claimsIdentity)
                                {
                                    foreach (var role in roles.EnumerateArray())
                                    {
                                        var roleString = role.GetString();
                                        if (!string.IsNullOrEmpty(roleString))
                                        {
                                            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, roleString));
                                        }
                                    }
                                }
                            }
                        }
                        catch (JsonException)
                        {
                            // Invalid JSON in realm_access; ignore roles extraction
                        }
                    }
                    return Task.CompletedTask;
                }
            };
        });

        services.AddAuthorization();
    }

    public static async Task InitializeCacheAsync(this IServiceProvider appServices)
    {
        using var scope = appServices.CreateScope();
        var services = scope.ServiceProvider;
        try
        {
            var globalizationService = services.GetRequiredService<IGlobalizationService>();

            await globalizationService.InitializeCacheAsync();

            OkMessage(MessageSource.Globalization, "Globalization cache initialized successfully.");
        }
        catch (Exception ex)
        {
            ErrorMessage(MessageSource.Globalization, "CRITICAL ERROR when initializing globalization cache:");
            ErrorMessage(MessageSource.Error, ex.Message);
        }
    }

    public static async Task CreateDummyDataAsync(this IServiceProvider appServices)
    {
        try
        {
            using var scope = appServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<StockDbContext>();
            await SeedBoxesAsync(context);
            await SeedItemsAsync(context);
            await SeedStorageAsync(context);
        }
        catch (Exception ex)
        {
            ErrorMessage(MessageSource.DataBase, "CRITICAL ERROR when adding database data:");
            ErrorMessage(MessageSource.Error, ex.Message);
        }
        finally
        {
            Console.ResetColor();
        }
    }

    private static async Task SeedBoxesAsync(StockDbContext context)
    {
        InfoMessage(MessageSource.DataBase, "Checking boxes...");

        if (await context.Boxes.AnyAsync())
        {
            InfoMessage(MessageSource.DataBase, "Boxes without change.");
            return;
        }

        var initialBoxes = new List<Box>
        {
            new() {
                Name = "DEWALT TSTAK VI",
                CategoryId = 7,
                BrandId = 5,
                Height = 30.2m,
                Width = 44.0m,
                Depth = 33.3m,
                Notes = "Caja de gran volumen para herramientas eléctricas.",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            },
            new() {
                Name = "DEWALT ToughSystem 2.0",
                CategoryId = 7,
                BrandId = 5,
                Height = 40.8m,
                Width = 55.4m,
                Depth = 37.1m,
                Notes = "Caja de alta resistencia, sellado IP65.",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            },
            new() {
                Name = "DEWALT Small Parts Organizer",
                CategoryId = 7,
                BrandId = 5,
                Height = 11.5m,
                Width = 44.0m,
                Depth = 33.3m,
                Notes = "Organizador con contenedores extraíbles para tornillería.",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            }
        };

        await context.Boxes.AddRangeAsync(initialBoxes);
        await context.SaveChangesAsync();

        OkMessage(MessageSource.DataBase, "Boxes ready");
    }

    private static async Task SeedItemsAsync(StockDbContext context)
    {
        InfoMessage(MessageSource.DataBase, "Checking items...");

        if (await context.Items.AnyAsync())
        {
            InfoMessage(MessageSource.DataBase, "Items without change.");
            return;
        }

        var items = new List<Item>
        {
            new() { Name = "20V Drill", CategoryId = 7, Notes = "---", UpdatedAt = DateTimeOffset.UtcNow, CreatedAt = DateTimeOffset.UtcNow },
            new() { Name = "Earphones", CategoryId = 4, Notes = "---", UpdatedAt = DateTimeOffset.UtcNow, CreatedAt = DateTimeOffset.UtcNow },
            new() { Name = "Mouse", CategoryId = 4, Notes = "---", UpdatedAt = DateTimeOffset.UtcNow, CreatedAt = DateTimeOffset.UtcNow },
            new() { Name = "Pencils", CategoryId = 5, Notes = "---", UpdatedAt = DateTimeOffset.UtcNow, CreatedAt = DateTimeOffset.UtcNow },
            new() { Name = "AA Batteries", CategoryId = 2, Notes = "---", UpdatedAt = DateTimeOffset.UtcNow, CreatedAt = DateTimeOffset.UtcNow }
        };

        await context.Items.AddRangeAsync(items);
        await context.SaveChangesAsync();

        OkMessage(MessageSource.DataBase, "Items ready");
    }

    private static async Task SeedStorageAsync(StockDbContext context)
    {
        InfoMessage(MessageSource.DataBase, "Checking storage relations...");

        if (await context.Storages.AnyAsync())
        {
            InfoMessage(MessageSource.DataBase, "Storage already populated.");
            return;
        }

        var boxTStak = await context.Boxes.FirstOrDefaultAsync(b => b.Name.Contains("TSTAK"));
        var boxOrganizer = await context.Boxes.FirstOrDefaultAsync(b => b.Name.Contains("Organizer"));

        // 2. Buscamos los ítems específicos
        var itemDrill = await context.Items.FirstOrDefaultAsync(i => i.Name.Contains("20V Drill"));
        var itemBatteries = await context.Items.FirstOrDefaultAsync(i => i.Name.Contains("Batteries"));
        var itemPencils = await context.Items.FirstOrDefaultAsync(i => i.Name.Contains("Pencils"));

        if (boxTStak == null || boxOrganizer == null || itemDrill == null || itemBatteries == null || itemPencils == null)
        {
            ErrorMessage(MessageSource.DataBase, "Could not find Boxes or Items to relate in Storage.");
            return;
        }

        var initialStorage = new List<Storage>
        {
            new() {
                BoxId = boxTStak.BoxId,
                ItemId = itemDrill.ItemId,
                BrandId = 5,
                Quantity = 1,
                Expires = false,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            },
            new() {
                BoxId = boxOrganizer.BoxId,
                ItemId = itemBatteries!.ItemId,
                BrandId = 8,
                Quantity = 8,
                Expires = true,
                ExpiresOn = new DateOnly(2028, 05, 20),
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            },
            new() {
                BoxId = boxOrganizer.BoxId,
                ItemId = itemPencils!.ItemId,
                BrandId = 2,
                Quantity = 12,
                Expires = false,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            },
            new() {
                BoxId = boxOrganizer.BoxId,
                ItemId = itemBatteries!.ItemId,
                BrandId = 0,
                Quantity = 12,
                Expires = true,
                ExpiresOn = new DateOnly(2027, 10, 15),
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            }
        };

        await context.Storages.AddRangeAsync(initialStorage);
        await context.SaveChangesAsync();

        OkMessage(MessageSource.DataBase, "Storage relations ready");
    }

    private static void WriteMessage(ConsoleColor color, MessageSource source, string message)
    {
        switch (source)
        {
            case MessageSource.DependencyInjection:
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("DI");
                break;
            case MessageSource.DataBase:
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("DB");
                break;
            case MessageSource.Globalization:
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("GL");
                break;
            case MessageSource.Error:
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("ER");
                break;
        }
        Console.ResetColor();
        if (source != MessageSource.None) { Console.Write(":\\> "); }
        else { Console.Write("      "); }
        Console.ForegroundColor = color;
        Console.WriteLine(message);
    }
    private static void OkMessage(MessageSource source, string message)
    {
        WriteMessage(ConsoleColor.Green, source, message);
    }
    private static void InfoMessage(MessageSource source, string message)
    {
        WriteMessage(ConsoleColor.Cyan, source, message);
    }
    private static void ErrorMessage(MessageSource source, string message)
    {
        WriteMessage(ConsoleColor.Red, source, message);
    }

}