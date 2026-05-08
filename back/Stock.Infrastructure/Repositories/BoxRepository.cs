using Microsoft.EntityFrameworkCore;
using Stock.Application.Common;
using Stock.Application.Interfaces.Common;
using Stock.Domain.Entities;
using Stock.Domain.Entities.Views;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Persistence;
using Stock.Infrastructure.Services;
using static Stock.Foundation.Common.SystemRegistry;

namespace Stock.Infrastructure.Repositories;

public class BoxRepository(StockDbContext context, IAuditFactory auditFactory) : IBoxRepository
{
    /// <inheritdoc />
    public async Task<Box?> FindAsync(int boxId)
    {
        return await context.Boxes.FindAsync(boxId);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(int boxId)
    {
        return await context.Boxes.AnyAsync(x => x.BoxId == boxId);
    }

    /// <inheritdoc />
    public async Task<bool> HasChildrenAsync(int boxId)
    {
        return await context.Boxes.AnyAsync(x => x.ParentBoxId == boxId);
    }

    /// <inheritdoc />
    public async Task<bool> HasItemsAsync(int boxId)
    {
        return await context.Storages.AnyAsync(x => x.BoxId == boxId);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(string name, int? parentBoxId, int? excludeId = null)
    {
        return await context.Boxes.AnyAsync(b =>
            b.Name == name &&
            b.ParentBoxId == parentBoxId &&
            (!excludeId.HasValue || b.BoxId != excludeId.Value));
    }

    /// <inheritdoc />
    public async Task<BoxDetailed?> GetBoxByIdAsync(int boxId)
    {
        var results = await context.Database
            .SqlQuery<BoxDetailed>($"EXEC [dbo].[GetBoxById] @BoxId = {boxId}")
            .ToListAsync();

        return results.FirstOrDefault();
    }

    /// <inheritdoc />
    public async Task<string?> GetBoxFullPathAsync(int boxId)
    {
        var results = await context.Database
            .SqlQuery<string>($"EXEC [dbo].[GetBoxFullPath] @BoxId = {boxId}")
            .ToListAsync();

        return results.FirstOrDefault();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BoxList>> GetBoxesByParentAsync(int? parentBoxId)
    {
        return await context.Database
            .SqlQuery<BoxList>($"EXEC [dbo].[GetBoxesByParent] @ParentBoxId = {parentBoxId}")
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BoxLookupList>> GetBoxesLookupAsync()
    {
        return await context.Database
            .SqlQuery<BoxLookupList>($"EXEC [dbo].[GetBoxesLookup]")
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<string?> GetBoxFullPathByParentAsync(int? parentBoxId)
    {
        var results = await context.Database
                .SqlQuery<string>($"EXEC [dbo].[GetBoxFullPathByParent] @ParentBoxId = {parentBoxId}")
                .ToListAsync();

        return results.FirstOrDefault();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BoxTransferList>> GetAvailableParentBoxesByAsync(int? targetBoxId)
    {
        return await context.Database
            .SqlQuery<BoxTransferList>($"EXEC [dbo].[GetAvailableParentBoxes] @TargetBoxId = {targetBoxId}")
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<bool> MoveBoxAsync(int boxId, int? newParentBoxId)
    {
        var box = await context.Boxes
            .Select(b => new { b.BoxId, b.ParentBoxId })
            .FirstOrDefaultAsync(b => b.BoxId == boxId);

        if (box == null) return false;

        using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var rows = await context.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC [dbo].[MoveBox] @BoxId = {boxId}, @NewParentBoxId = {newParentBoxId}"
            );

            if (rows > 0)
            {                
                var auditRequest = new AuditRequest
                {
                    EntityId = Entity.Box,
                    EventId = Event.Move,
                    RecordId = boxId.ToString(),
                    OldValues = new Dictionary<string, object?> { ["ParentBoxId"] = box.ParentBoxId },
                    NewValues = new Dictionary<string, object?> { ["ParentBoxId"] = newParentBoxId }
                };
                
                var auditEntity = auditFactory.Create(auditRequest);
                context.Audits.Add(auditEntity);

                await context.SaveChangesAsync();
            }

            await transaction.CommitAsync();
            return rows > 0;
        }
        catch
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    /// <inheritdoc />
    public async Task<int> AddAsync(Box box)
    {
        context.Boxes.Add(box);
        await context.SaveChangesAsync();
        return box.BoxId;
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(Box box)
    {
        context.Entry(box).State = EntityState.Modified;
        int updated = await context.SaveChangesAsync();
        return updated > 0;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(Box box)
    {
        context.Boxes.Remove(box);
        int deleted = await context.SaveChangesAsync();
        return deleted > 0;
    }

    /// <inheritdoc />
    public async Task<DateTimeOffset> ChangeImageAtAsync(int boxId)
    {
        var box = await context.Boxes
            .Select(b => new { b.BoxId, b.ImageAt })
            .FirstOrDefaultAsync(b => b.BoxId == boxId);

        var now = DateTimeOffset.UtcNow;

        if (box == null) return now;

        await context.Boxes
            .Where(b => b.BoxId == boxId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(b => b.ImageAt, now));

        var auditRequest = new AuditRequest
        {
            EntityId = Entity.Box,
            EventId = Event.UpdateImage,
            RecordId = boxId.ToString(),
            OldValues = new Dictionary<string, object?> { ["ImageAt"] = box.ImageAt },
            NewValues = new Dictionary<string, object?> { ["ImageAt"] = now }
        };

        var auditEntity = auditFactory.Create(auditRequest);
        context.Audits.Add(auditEntity);

        await context.SaveChangesAsync();

        return now;
    }
}
