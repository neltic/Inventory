using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Domain.Entities.Views;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Persistence;
using System.Data;

namespace Stock.Infrastructure.Repositories;

public class StorageRepository(StockDbContext context) : IStorageRepository
{
    /// <inheritdoc />
    public async Task<IEnumerable<ItemInBoxList>> GetItemsByBoxIdAsync(int boxId)
    {
        return await context.Storages
            .Where(s => s.BoxId == boxId)
            .OrderBy(s => s.Item.Name)
            .ThenByDescending(s => s.Quantity)
            .Select(s => new ItemInBoxList(
                s.ItemId,
                s.Item.Name,
                s.BrandId,
                s.Item.CategoryId,
                s.Quantity,
                s.Item.ImageAt,
                s.Notes
            ))
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<bool> RemoveAsync(int boxId, int itemId, int brandId)
    {
        var storage = await context.Storages
            .FirstOrDefaultAsync(s => s.BoxId == boxId
                                   && s.ItemId == itemId
                                   && s.BrandId == brandId);

        if (storage == null) return false;

        context.Storages.Remove(storage);

        var rows = await context.SaveChangesAsync();

        return rows > 0;
    }

    /// <inheritdoc />
    public async Task<Storage?> GetStorageAsync(int boxId, int itemId, int brandId)
    {
        return await context.Storages
            .AsNoTracking()
            .Where(s => s.BoxId == boxId && s.ItemId == itemId && s.BrandId == brandId)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ItemStorageList>> GetStorageByItemIdAsync(int itemId)
    {
        return await context.Database
                   .SqlQuery<ItemStorageList>($"EXEC [dbo].[GetStorageByItemId] @ItemId = {itemId}")
                   .ToListAsync();
    }

    /// <inheritdoc />    
    public async Task<bool> SetStorageAsync(Storage storage)
    {
        var existingStorage = await context.Storages
            .FirstOrDefaultAsync(s => s.BoxId == storage.BoxId
                                   && s.ItemId == storage.ItemId
                                   && s.BrandId == storage.BrandId);

        if (existingStorage != null)
        {
            if (storage.Quantity <= 0)
            {
                context.Storages.Remove(existingStorage);
            }
            else
            {
                existingStorage.Quantity = storage.Quantity;
                existingStorage.Expires = storage.Expires;
                existingStorage.ExpiresOn = storage.Expires ? storage.ExpiresOn : null;
                existingStorage.Notes = storage.Notes;

                context.Storages.Update(existingStorage);
            }
        }
        else
        {
            if (storage.Quantity > 0)
            {
                storage.ExpiresOn = storage.Expires ? storage.ExpiresOn : null;
                await context.Storages.AddAsync(storage);
            }
            else
            {
                return false;
            }
        }

        var rows = await context.SaveChangesAsync();
        return rows > 0;
    }
}