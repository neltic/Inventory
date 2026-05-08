using Microsoft.EntityFrameworkCore;
using Stock.Application.Interfaces.Common;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Persistence;
using Stock.Infrastructure.Persistence.Common;
using static Stock.Foundation.Common.SystemRegistry;

namespace Stock.Infrastructure.Repositories;

public class CategoryRepository(StockDbContext context, ICurrentUserService currentUserService) : ICategoryRepository
{
    /// <inheritdoc />
    public async Task<bool> ExistsAsync(int categoryId) =>
        await context.Categories.AnyAsync(x => x.CategoryId == categoryId);

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(string name, int? excludeId = null) =>
        await context.Categories.AnyAsync(x => x.Name == name &&
            (!excludeId.HasValue || x.CategoryId != excludeId.Value));

    /// <inheritdoc />
    public async Task<Category?> GetByIdAsync(int categoryId) =>
        await context.Categories.FindAsync(categoryId);

    /// <inheritdoc />
    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await context.Categories
            .AsNoTracking()
            .OrderBy(c => c.Order)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<int> AddAsync(Category category)
    {
        context.Categories.Add(category);
        await context.SaveChangesAsync();
        return category.CategoryId;
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(Category category)
    {
        context.Entry(category).State = EntityState.Modified;
        return await context.SaveChangesAsync() > 0;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int categoryId)
    {
        using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            var category = await context.Categories.FindAsync(categoryId);

            if (category == null) return false;

            context.Categories.Remove(category);
            await context.SaveChangesAsync();

            await context.Database.ExecuteSqlRawAsync("EXEC [dbo].[OrganizeCategories]");

            await transaction.CommitAsync();

            return true;
        }
        catch
        {
            await transaction.RollbackAsync();

            return false;
        }
    }

    /// <inheritdoc />    
    public async Task<bool> ReorderAsync(int categoryId, int newOrder)
    {
        var category = await context.Categories
            .Select(c => new { c.CategoryId, c.Order })
            .FirstOrDefaultAsync(c => c.CategoryId == categoryId);

        if (category == null) return false;

        using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            await context.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC [dbo].[ReorderCategory] @CategoryId = {categoryId}, @NewOrder = {newOrder}"
            );

            var entry = context.Entry(category);
            var auditEntry = new AuditEntry(entry)
            {
                EntityId = Entity.Category,
                EventId = Event.Reorder,
                RecordId = categoryId.ToString(),
                By = currentUserService.Username,
                At = DateTimeOffset.UtcNow,
                UserSnapshot = currentUserService.GetInfo()
            };

            auditEntry.OldValues["Order"] = category.Order;
            auditEntry.NewValues["Order"] = newOrder;

            context.Audits.Add(auditEntry.ToAuditEntity());
            await context.SaveChangesAsync();

            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            return false;
        }
    }
}