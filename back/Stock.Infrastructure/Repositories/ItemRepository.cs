using Microsoft.EntityFrameworkCore;
using Stock.Application.Common;
using Stock.Application.Interfaces.Common;
using Stock.Domain.Entities;
using Stock.Domain.Entities.Views;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Persistence;
using static Stock.Foundation.Common.SystemRegistry;

namespace Stock.Infrastructure.Repositories;

public class ItemRepository(StockDbContext context, IAuditFactory auditFactory) : IItemRepository
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
                i.ImageAt,
                i.Storages.Any()
            ))
            .ToListAsync();

    /// <inheritdoc />
    public async Task<IEnumerable<ItemLocationList>> GetItemLocationAsync(int itemId)
    {
        return await context.Database
               .SqlQuery<ItemLocationList>($"EXEC [dbo].[GetItemLocation] @ItemId = {itemId}")
               .AsNoTracking()
               .ToListAsync();
    }

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
    public async Task<DateTimeOffset> ChangeImageAtAsync(int itemId)
    {
        var item = await context.Items
           .Select(i => new { i.ItemId, i.ImageAt })
           .FirstOrDefaultAsync(i => i.ItemId == itemId);

        var now = DateTimeOffset.UtcNow;

        if (item == null) return now;

        await context.Items
            .Where(b => b.ItemId == itemId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(b => b.ImageAt, now));

        var auditRequest = new AuditRequest
        {
            EntityId = Entity.Item,
            EventId = Event.UpdateImage,
            RecordId = itemId.ToString(),
            OldValues = new Dictionary<string, object?> { ["ImageAt"] = item.ImageAt },
            NewValues = new Dictionary<string, object?> { ["ImageAt"] = now }
        };

        var auditEntity = auditFactory.Create(auditRequest);
        context.Audits.Add(auditEntity);

        await context.SaveChangesAsync();

        return now;
    }
}