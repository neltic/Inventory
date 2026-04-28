using Stock.Application.DTOs;
using Stock.Application.Interfaces;
using Stock.Application.Mappings;
using Stock.Domain.Interfaces;

namespace Stock.Application.Services;

public class StorageService(IStorageRepository storageRepository) : IStorageService
{
    /// <inheritdoc />
    public async Task<IEnumerable<ItemInBoxListDto>> GetItemsByBoxIdAsync(int boxId)
    {
        var results = await storageRepository.GetItemsByBoxIdAsync(boxId);

        return results.ToDtoList();
    }

    /// <inheritdoc />
    public async Task<bool> RemoveAsync(int boxId, int itemId, int brandId) =>
        await storageRepository.RemoveAsync(boxId, itemId, brandId);

    /// <inheritdoc />
    public async Task<StorageDto> GetStorageAsync(int boxId, int itemId, int brandId)
    {
        var result = await storageRepository.GetStorageAsync(boxId, itemId, brandId);
        return result != null ? result.ToDto() : new(boxId, itemId, brandId, -1, false, null, null);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ItemStorageListDto>> GetStorageByItemIdAsync(int itemId)
    {
        var results = await storageRepository.GetStorageByItemIdAsync(itemId);

        return results.ToDtoList();
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(StorageDto dto)
    {
        var storage = dto.ToEntity();

        return await storageRepository.UpdateAsync(storage);
    }
}