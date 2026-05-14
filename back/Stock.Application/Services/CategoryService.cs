using Stock.Application.DTOs;
using Stock.Application.Interfaces;
using Stock.Application.Interfaces.Common;
using Stock.Application.Mappings;
using Stock.Domain.Interfaces;
using Stock.Foundation.Common;

namespace Stock.Application.Services;

public class CategoryService(ICategoryRepository categoryRepository, ICacheService cache)
    : BaseCacheService("catalog", "category"), ICategoryService
{
    /// <inheritdoc />
    public async Task<CategoryDto?> GetByIdAsync(int categoryId)
    {
        if (categoryId <= 0) return null;

        return await cache.GetOrSetAsync(GetCacheKeyItem(categoryId), async () =>
        {
            var category = await categoryRepository.GetByIdAsync(categoryId);
            return category?.ToDto();
        });
    }

    /// <inheritdoc />
    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        return await cache.GetOrSetAsync(CacheKeyList, async () =>
        {
            var results = await categoryRepository.GetAllAsync();
            return results.ToDtoList();
        });
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(CategoryDto dto)
    {
        var exists = await categoryRepository.ExistsAsync(dto.Name);
        if (exists) throw new InvalidOperationException(LabelRegistry.Key.AlreadyExists);

        var category = dto.ToEntity(0);
        var categoryId = await categoryRepository.AddAsync(category);

        await cache.RemoveAsync(CacheKeyList);

        return categoryId;
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(int categoryId, CategoryDto dto)
    {
        var existingCategory = await categoryRepository.ExistsAsync(categoryId);
        if (!existingCategory) return false;

        var exists = await categoryRepository.ExistsAsync(dto.Name, dto.CategoryId);
        if (exists) throw new InvalidOperationException(LabelRegistry.Key.AlreadyExists);

        var current = await categoryRepository.GetByIdAsync(categoryId);

        var result = await categoryRepository.UpdateAsync(dto.ToEntity(categoryId, current));

        if (result) await cache.RemoveAsync(GetCacheKeyItem(categoryId), CacheKeyList);

        return result;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int categoryId)
    {
        var result = await categoryRepository.DeleteAsync(categoryId);

        if (result) await cache.RemoveAsync(GetCacheKeyItem(categoryId), CacheKeyList);

        return result;
    }

    /// <inheritdoc />
    public async Task<bool> ReorderAsync(int categoryId, int newOrder)
    {
        var result = await categoryRepository.ReorderAsync(categoryId, newOrder);

        if (result) await cache.RemoveAsync(GetCacheKeyItem(categoryId), CacheKeyList);

        return result;
    }
}
