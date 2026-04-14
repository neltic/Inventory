using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Stock.Application.Common;
using Stock.Application.Interfaces.Common;
using Stock.Foundation.Common;

namespace Stock.Infrastructure.Services;

public class FileStorageService(IOptions<FileStorageOptions> options) : IFileStorageService
{
    private readonly FileStorageOptions _options = options.Value;

    /// <inheritdoc />
    public async Task<string> SaveTempImageAsync(Stream fileStream)
    {
        var fileName = FileRegistry.GetRandomImageName(out string guid);
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
        return await AssignImageToAsync(tempFileName, boxId, _options.BoxPath, FileRegistry.Folder.Box);
    }

    /// <inheritdoc />
    public async Task<bool> AssignImageToItemAsync(string tempFileName, int itemId)
    {
        return await AssignImageToAsync(tempFileName, itemId, _options.ItemPath, FileRegistry.Folder.Item);
    }

    /// <inheritdoc />
    public Task DeleteBoxImagesAsync(int boxId)
    {
        return MoveToTempAsync(boxId, FileRegistry.Folder.Box, _options.BoxPath);
    }

    /// <inheritdoc />
    public Task DeleteItemImagesAsync(int itemId)
    {
        return MoveToTempAsync(itemId, FileRegistry.Folder.Item, _options.ItemPath);
    }

    private async Task<bool> AssignImageToAsync(string tempFileName, int id, string idPath, string origin)
    {
        var tempPath = Path.Combine(_options.TempPath, FileRegistry.GetImageName(tempFileName));

        if (!File.Exists(tempPath)) return false;

        var finalPath = await GetCleanPath(id, Path.Combine(idPath, FileRegistry.Folder.SubFolder.Original));

        File.Move(tempPath, finalPath);

        // Create small images
        using var image = await Image.LoadAsync(finalPath);

        await CreateResizedImage(image, await GetCleanPath(id, Path.Combine(idPath, FileRegistry.Folder.SubFolder.Thumbnails)), new Size(480, 320));

        await CreateResizedImage(image, await GetCleanPath(id, Path.Combine(idPath, FileRegistry.Folder.SubFolder.Icons)), new Size(128, 128));

        // Create notification to syncronize with the cloud backup worker (linux only)
        if (OperatingSystem.IsLinux())
        {
            var syncPath = Path.Combine(_options.TempPath, FileRegistry.GetSyncFileName(origin, id));
            await File.WriteAllTextAsync(syncPath, string.Empty);
        }

        return true;
    }

    private static async Task<string> GetCleanPath(int id, string idPath)
    {
        Directory.CreateDirectory(idPath);

        var path = Path.Combine(idPath, FileRegistry.GetImageName(id));

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
        foreach (var folder in FileRegistry.Folder.SubFolder.All)
        {
            var sourcePath = Path.Combine(idPath, folder, FileRegistry.GetImageName(id));

            if (File.Exists(sourcePath))
            {
                var tempFileName = FileRegistry.GetDeletedImageName(origin, id, folder);
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