using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Stock.Application.Common;
using Stock.Application.Interfaces.Common;

namespace Stock.Infrastructure.Services;

public class FileStorageService : IFileStorageService
{
    private readonly FileStorageOptions _options;

    public FileStorageService(IOptions<FileStorageOptions> options)
    {
        _options = options.Value;
    }

    /// <inheritdoc />
    public async Task<string> SaveTempImageAsync(Stream fileStream)
    {
        var guid = Guid.NewGuid().ToString();
        var fileName = $"{guid}.png";
        var fullPath = Path.Combine(_options.TempPath, fileName);
        var directory = Path.GetDirectoryName(fullPath);

        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        using var image = await Image.LoadAsync(fileStream);

        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Size = new Size(1280, 720),
            Mode = ResizeMode.Max
        }));

        await image.SaveAsPngAsync(fullPath);

        return guid;
    }

    /// <inheritdoc />
    public async Task<bool> AssignImageToBoxAsync(string tempFileName, int boxId)
    {
        return await AssignImageToAsync(tempFileName, boxId, _options.BoxPath);
    }

    /// <inheritdoc />
    public async Task<bool> AssignImageToItemAsync(string tempFileName, int itemId)
    {
        return await AssignImageToAsync(tempFileName, itemId, _options.ItemPath);
    }
        
    private async Task<bool> AssignImageToAsync(string tempFileName, int id, string idPath)
    {
        var tempPath = Path.Combine(_options.TempPath, $"{tempFileName}.png");

        if (!File.Exists(tempPath)) return false;
        
        var finalPath = await GetCleanPath(id, Path.Combine(idPath, "original"));

        File.Move(tempPath, finalPath);

        // Create small images
        using var image = await Image.LoadAsync(finalPath);

        await CreateResizedImage(image, await GetCleanPath(id, Path.Combine(idPath, "thumbnails")), new Size(480, 320));

        await CreateResizedImage(image, await GetCleanPath(id, Path.Combine(idPath, "icons")), new Size(128, 128));

        return true;
    }
        
    private async Task<string> GetCleanPath(int id, string idPath)
    {
        Directory.CreateDirectory(idPath);

        var path = Path.Combine(idPath, $"{id}.png");

        if (File.Exists(path)) File.Delete(path);

        return path;
    }
        
    private async Task CreateResizedImage(Image image, string fullPath, Size size)
    {
        using (var newImage = image.Clone(x => x.Resize(new ResizeOptions { Size = size, Mode = ResizeMode.Max })))
        {
            await newImage.SaveAsync(fullPath);
        }
    }
}