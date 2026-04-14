using Stock.Application.DTOs;
using Stock.Application.Interfaces;
using Stock.Application.Interfaces.Common;
using Stock.Application.Mappings;
using Stock.Domain.Interfaces;

namespace Stock.Application.Services;

public class CategoryService(ICategoryRepository categoryRepository, ICacheService cache)
    : BaseCacheService("category"), ICategoryService
{
    /// <inheritdoc />
    public async Task<CategoryDto?> GetByIdAsync(int categoryId)
    {
        if (categoryId <= 0) return null;

        string key = GetCacheKeyItem(categoryId);

        var dto = await cache.GetAsync<CategoryDto>(key);

        if (dto != null) return dto;

        var category = await categoryRepository.GetByIdAsync(categoryId);
        dto = category?.ToDto();

        if (dto != null)
        {
            await cache.SetAsync(key, dto);
        }

        return dto;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        var cacheList = await cache.GetAsync<IEnumerable<CategoryDto>>(CacheKeyList);

        if (cacheList != null) return cacheList;


        var results = await categoryRepository.GetAllAsync();
        var dtoList = results.ToDtoList();

        await cache.SetAsync(CacheKeyList, dtoList);

        return dtoList;
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(CategoryDto dto)
    {
        var exists = await categoryRepository.ExistsAsync(dto.Name);
        if (exists) throw new InvalidOperationException("A category with this name already exists.");

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
        if (exists) throw new InvalidOperationException("A category with this name already exists.");

        var result = await categoryRepository.UpdateAsync(dto.ToEntity(categoryId));

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
