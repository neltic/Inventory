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
                s.Item.UpdatedAt
            ))
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<bool> RemoveAsync(int boxId, int itemId, int brandId)
    {
        var rows = await context.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC [dbo].[RemoveItemFromBox] @BoxId = {boxId}, @ItemId = {itemId}, @BrandId = {brandId}");

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
    public async Task<bool> UpdateAsync(Storage storage)
    {
        var rows = await context.Database.ExecuteSqlInterpolatedAsync(
                $"[dbo].[AddOrEditStorage] @BoxId = {storage.BoxId}, @ItemId = {storage.ItemId}, @BrandId = {storage.BrandId}, @Quantity = {storage.Quantity}, @Expires = {storage.Expires}, @ExpiresOn = {(storage.Expires ? storage.ExpiresOn : null)}, @Notes = {storage.Notes}"
                );

        return rows > 0;
    }
}