using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Domain.Entities.Views;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Persistence;

namespace Stock.Infrastructure.Repositories;

public class CategoryRepository(StockDbContext context) : ICategoryRepository
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
        try
        {
            await context.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC [dbo].[DeleteCategory] @CategoryId = {categoryId}"
            );

            return true;
        }
        catch 
        {            
            return false;
        }
    }

    /// <inheritdoc />
    public async Task<bool> ReorderAsync(int categoryId, int newOrder)
    {
        try
        {
            await context.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC [dbo].[ReorderCategory] @CategoryId = {categoryId}, @NewOrder = {newOrder}"
            );

            return true;
        }
        catch
        {
            return false;
        }
    }
}