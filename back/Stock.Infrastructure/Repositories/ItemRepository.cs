using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Domain.Entities.Views;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Persistence;

namespace Stock.Infrastructure.Repositories;

public class ItemRepository(StockDbContext context) : IItemRepository
{
    /// <inheritdoc />
    public async Task<Item?> FindAsync(int itemId) =>
        await context.Items.FindAsync(itemId);

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(int itemId) =>
        await context.Items.AnyAsync(x => x.ItemId == itemId);

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(string name, int? excludeId = null) =>
        await context.Items.AnyAsync(x => x.Name == name && (!excludeId.HasValue || x.ItemId != excludeId.Value));

    /// <inheritdoc />
    public async Task<ItemDetailed?> GetItemByIdAsync(int itemId)
    {
        var results = await context.Database
               .SqlQuery<ItemDetailed>($"EXEC [dbo].[GetItemById] @ItemId = {itemId}")
               .ToListAsync();

        return results.FirstOrDefault();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<ItemList>> GetItemsAsync() =>
        await context.Items
            .AsNoTracking()
            .OrderBy(i => i.Name)
            .Select(i => new ItemList(
                i.ItemId,
                i.Name,
                i.CategoryId,
                i.UpdatedAt,
                i.Storages.Any()
            ))
            .ToListAsync();

    /// <inheritdoc />
    public async Task<int> AddAsync(Item item)
    {
        context.Items.Add(item);
        await context.SaveChangesAsync();
        return item.ItemId;
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(Item item)
    {
        context.Entry(item).State = EntityState.Modified;
        return await context.SaveChangesAsync() > 0;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(Item item)
    {
        context.Items.Remove(item);
        return await context.SaveChangesAsync() > 0;
    }

    /// <inheritdoc />
    public async Task<DateTime> ChangeUpdatedAtAsync(int itemId)
    {
        var now = DateTime.UtcNow;

        await context.Items
            .Where(b => b.ItemId == itemId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(b => b.UpdatedAt, now));

        return now;
    }
}