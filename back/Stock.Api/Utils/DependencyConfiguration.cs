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
                ValidIssuer = keycloakOptions.Issuer,
                ClockSkew = TimeSpan.FromSeconds(keycloakOptions.ClockSkewSeconds),
                RoleClaimType = ClaimTypes.Role
            };
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = context =>
                {
                    var principal = context.Principal;
                    if (principal == null) return Task.CompletedTask;
                    var rAccess = principal.FindFirst("realm_access");
                    if (rAccess == null || string.IsNullOrEmpty(rAccess.Value)) return Task.CompletedTask;
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
                Name = "Tstak DWST17808",
                CategoryId = 7,
                BrandId = 5,
                Height = 18.50m,
                Width = 44.50m,
                Depth = 33.50m,
                Notes = "Additional handle on top of each unit for easy and comfortable lifting. Made of high-impact resistant polypropylene.",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            },
            new() {
                Name = "Tstak DWST17805",
                CategoryId = 7,
                BrandId = 5,
                Height = 11.70m,
                Width = 43.80m,
                Depth = 33.70m,
                Notes = "TSTAK organizer with transparent lid and removable compartments (2 large and 5 small)",
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            },
            new() {
                Name = "Tstak DWST17804",
                CategoryId = 7,
                BrandId = 5,
                Height = 17.50m,
                Width = 43.90m,
                Depth = 31.20m,
                Notes = "Platform that adapts to the different needs of the user. Sides of the organizer with systems to attach to other TSTAK boxes. Drawers with removable dividers to organize small parts and accessories. Removable dividers to store drill bits and tips. Built-in ergonomic handle to carry heavier loads.",
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
            new() { Name = "USB C → USB C", CategoryId = 4, Notes = "USB C → USB C", UpdatedAt = DateTimeOffset.UtcNow, CreatedAt = DateTimeOffset.UtcNow },
            new() { Name = "USB A → Mini USB", CategoryId = 4, Notes = "USB A → Mini USB", UpdatedAt = DateTimeOffset.UtcNow, CreatedAt = DateTimeOffset.UtcNow },
            new() { Name = "USB A → Micro USB", CategoryId = 4, Notes = "USB A → Micro USB", UpdatedAt = DateTimeOffset.UtcNow, CreatedAt = DateTimeOffset.UtcNow },
            new() { Name = "Webcam", CategoryId = 4, Notes = "Medium resolution webcam", UpdatedAt = DateTimeOffset.UtcNow, CreatedAt = DateTimeOffset.UtcNow },
            new() { Name = "USB A → USB C", CategoryId = 4, Notes = "USB A → USB C", UpdatedAt = DateTimeOffset.UtcNow, CreatedAt = DateTimeOffset.UtcNow }
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

        var boxDWST17808 = await context.Boxes.FirstOrDefaultAsync(b => b.Name.Contains("DWST17808"));
        var boxDWST17805 = await context.Boxes.FirstOrDefaultAsync(b => b.Name.Contains("DWST17805"));

        var itemCC = await context.Items.FirstOrDefaultAsync(i => i.Name.Contains("USB C → USB C"));
        var itemAMini = await context.Items.FirstOrDefaultAsync(i => i.Name.Contains("USB A → Mini USB"));
        var itemAMicro = await context.Items.FirstOrDefaultAsync(i => i.Name.Contains("USB A → Micro USB"));

        if (boxDWST17808 == null || boxDWST17805 == null || itemCC == null || itemAMini == null || itemAMicro == null)
        {
            ErrorMessage(MessageSource.DataBase, "Could not find Boxes or Items to relate in Storage.");
            return;
        }

        var initialStorage = new List<Storage>
        {
            new() {
                BoxId = boxDWST17808.BoxId,
                ItemId = itemCC.ItemId,
                BrandId = 5,
                Quantity = 1,
                Expires = false,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            },
            new() {
                BoxId = boxDWST17805.BoxId,
                ItemId = itemAMini!.ItemId,
                BrandId = 8,
                Quantity = 8,
                Expires = true,
                ExpiresOn = new DateOnly(2028, 05, 20),
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            },
            new() {
                BoxId = boxDWST17805.BoxId,
                ItemId = itemAMicro!.ItemId,
                BrandId = 2,
                Quantity = 12,
                Expires = false,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            },
            new() {
                BoxId = boxDWST17805.BoxId,
                ItemId = itemAMini!.ItemId,
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