using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Persistence;

namespace Stock.Infrastructure.Repositories;

public class BrandRepository(StockDbContext context) : IBrandRepository
{
    /// <inheritdoc />
    public async Task<Brand?> FindAsync(int BrandId) =>
        await context.Brands.FindAsync(BrandId);

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(int BrandId) =>
        await context.Brands.AnyAsync(x => x.BrandId == BrandId);

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(string name, int? excludeId = null) =>
        await context.Brands.AnyAsync(x => x.Name == name &&
            (!excludeId.HasValue || x.BrandId != excludeId.Value));

    /// <inheritdoc />
    public async Task<Brand?> GetByIdAsync(int BrandId) =>
        await context.Brands.FindAsync(BrandId);

    /// <inheritdoc />
    public async Task<IEnumerable<Brand>> GetAllAsync()
    {
        return await context.Brands
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<int> AddAsync(Brand Brand)
    {
        context.Brands.Add(Brand);
        await context.SaveChangesAsync();
        return Brand.BrandId;
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(Brand Brand)
    {
        context.Entry(Brand).State = EntityState.Modified;
        return await context.SaveChangesAsync() > 0;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(Brand Brand)
    {
        context.Brands.Remove(Brand);
        return await context.SaveChangesAsync() > 0;
    }
}