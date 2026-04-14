using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Stock.Application.Common;
using Stock.Application.Interfaces.Common;

namespace Stock.Infrastructure.Services;

public class FileStorageService(IOptions<FileStorageOptions> options) : IFileStorageService
{
    private readonly FileStorageOptions _options = options.Value;

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
        return await AssignImageToAsync(tempFileName, boxId, _options.BoxPath, "box");
    }

    /// <inheritdoc />
    public async Task<bool> AssignImageToItemAsync(string tempFileName, int itemId)
    {
        return await AssignImageToAsync(tempFileName, itemId, _options.ItemPath, "item");
    }

    /// <inheritdoc />
    public Task DeleteBoxImagesAsync(int boxId)
    {
        return MoveToTempAsync(boxId, "box", _options.BoxPath);
    }

    /// <inheritdoc />
    public Task DeleteItemImagesAsync(int itemId)
    {
        return MoveToTempAsync(itemId, "item", _options.ItemPath);
    }

    private async Task<bool> AssignImageToAsync(string tempFileName, int id, string idPath, string origin)
    {
        var tempPath = Path.Combine(_options.TempPath, $"{tempFileName}.png");

        if (!File.Exists(tempPath)) return false;

        var finalPath = await GetCleanPath(id, Path.Combine(idPath, "original"));

        File.Move(tempPath, finalPath);

        // Create small images
        using var image = await Image.LoadAsync(finalPath);

        await CreateResizedImage(image, await GetCleanPath(id, Path.Combine(idPath, "thumbnails")), new Size(480, 320));

        await CreateResizedImage(image, await GetCleanPath(id, Path.Combine(idPath, "icons")), new Size(128, 128));

        // Create notification to syncronize with the cloud backup worker (linux only)
        if (OperatingSystem.IsLinux())
        {
            var syncPath = Path.Combine(_options.TempPath, $"sync_{origin}_{id}.txt");
            await File.WriteAllTextAsync(syncPath, string.Empty);
        }

        return true;
    }

    private static async Task<string> GetCleanPath(int id, string idPath)
    {
        Directory.CreateDirectory(idPath);

        var path = Path.Combine(idPath, $"{id}.png");

        if (File.Exists(path)) File.Delete(path);

        return path;
    }

    private static async Task CreateResizedImage(Image image, string fullPath, Size size)
    {
        using var newImage = image.Clone(x => x.Resize(new ResizeOptions { Size = size, Mode = ResizeMode.Max }));
        await newImage.SaveAsync(fullPath);
    }

    /// <summary>
    /// Internal helper to relocate files from structured storage to the temp directory.
    /// This prevents immediate physical deletion and allows the background cleaner to handle disposal.
    /// </summary>
    /// <param name="id">The entity identifier (BoxId or ItemId).</param>
    /// <param name="origin">The source entity type for naming purposes (e.g., "box" or "item").</param>
    /// <param name="idPath">The base directory path where the source folders are located.</param>
    private async Task MoveToTempAsync(int id, string origin, string idPath)
    {
        string[] subFolders = ["original", "thumbnails", "icons"];

        foreach (var folder in subFolders)
        {
            var sourcePath = Path.Combine(idPath, folder, $"{id}.png");

            if (File.Exists(sourcePath))
            {
                var tempFileName = $"deleted_{origin}_{id}_{folder}_{Guid.NewGuid()}.png";
                var destinationPath = Path.Combine(_options.TempPath, tempFileName);

                try
                {
                    Directory.CreateDirectory(_options.TempPath);
                    File.Move(sourcePath, destinationPath);
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}