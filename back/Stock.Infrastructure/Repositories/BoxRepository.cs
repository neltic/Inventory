using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Domain.Entities.Views;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Persistence;

namespace Stock.Infrastructure.Repositories;

public class BoxRepository(StockDbContext context) : IBoxRepository
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
    public async Task<bool> MoveBoxAsync(int boxId, int? newParentId)
    {
        return await context.Database.ExecuteSqlInterpolatedAsync($" EXEC [dbo].[MoveBox] @BoxId = {boxId}, @NewParentId = {newParentId}") > 0;
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
    public async Task<DateTime> ChangeUpdatedAtAsync(int boxId)
    {
        var now = DateTime.UtcNow;

        await context.Boxes
            .Where(b => b.BoxId == boxId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(b => b.UpdatedAt, now));

        return now;
    }
}
