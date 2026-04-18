using Stock.Application.DTOs;
using Stock.Application.Interfaces;
using Stock.Application.Mappings;
using Stock.Domain.Interfaces;
using Stock.Foundation.Common;

namespace Stock.Application.Services;

public class ItemService(IItemRepository itemRepository) : IItemService
{
    /// <inheritdoc />
    public async Task<ItemDetailedDto?> GetItemByIdAsync(int itemId)
    {
        var item = await itemRepository.GetItemByIdAsync(itemId);
        return item?.ToDto();
    }

    /// <inheritdoc />
    public async Task<ItemDetailedDto?> GetEmptyItemAsync()
    {
        return new ItemDetailedDto(0, string.Empty, string.Empty, 0, DateTime.Today, DateTime.Today);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ItemListDto>> GetItemsAsync()
    {
        var results = await itemRepository.GetItemsAsync();
        return results.ToDtoList();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ItemLocationListDto>> GetItemLocationAsync(int itemId)
    {
        var results = await itemRepository.GetItemLocationAsync(itemId);
        return results.ToDtoList();
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(ItemDto dto)
    {
        var exists = await itemRepository.ExistsAsync(dto.Name);

        if (exists)
        {
            throw new InvalidOperationException(LabelRegistry.Key.AlreadyExists);
        }

        var entity = dto.ToEntity(0);

        return await itemRepository.AddAsync(entity);
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(int itemId, ItemDto dto)
    {
        var existingItem = await itemRepository.ExistsAsync(itemId);

        if (!existingItem)
        {
            return false;
        }

        var exists = await itemRepository.ExistsAsync(dto.Name, dto.ItemId);

        if (exists)
        {
            throw new InvalidOperationException(LabelRegistry.Key.AlreadyExists);
        }

        return await itemRepository.UpdateAsync(dto.ToEntity(itemId));
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int itemId)
    {
        var item = await itemRepository.FindAsync(itemId);
        if (item == null) return false;

        return await itemRepository.DeleteAsync(item);
    }

    /// <inheritdoc />
    public async Task<DateTime> ChangeUpdatedAtAsync(int itemId)
    {
        return await itemRepository.ChangeUpdatedAtAsync(itemId);
    }
}