using Stock.Application.DTOs;
using Stock.Application.Interfaces;
using Stock.Application.Interfaces.Common;
using Stock.Application.Mappings;
using Stock.Domain.Interfaces;
using Stock.Foundation.Common;

namespace Stock.Application.Services;

public class BrandService(IBrandRepository brandRepository, ICacheService cache)
    : BaseCacheService("catalog", "brand"), IBrandService
{
    /// <inheritdoc />
    public async Task<BrandDto?> GetByIdAsync(int brandId)
    {
        if (brandId <= 0) return null;

        string key = GetCacheKeyItem(brandId);

        var dto = await cache.GetAsync<BrandDto>(key);
        if (dto != null) return dto;

        var brand = await brandRepository.GetByIdAsync(brandId);
        dto = brand?.ToDto();

        if (dto != null)
        {
            await cache.SetAsync(key, dto);
        }

        return dto;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BrandDto>> GetAllAsync()
    {
        var cacheList = await cache.GetAsync<IEnumerable<BrandDto>>(CacheKeyList);
        if (cacheList != null) return cacheList;

        var results = await brandRepository.GetAllAsync();
        var dtoList = results.ToDtoList();

        await cache.SetAsync(CacheKeyList, dtoList);

        return dtoList;
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(BrandDto dto)
    {
        var exists = await brandRepository.ExistsAsync(dto.Name);
        if (exists) throw new InvalidOperationException(LabelRegistry.Key.AlreadyExists);

        var brand = dto.ToEntity(0);
        var brandId = await brandRepository.AddAsync(brand);

        await cache.RemoveAsync(CacheKeyList);

        return brandId;
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(int brandId, BrandDto dto)
    {
        var existingBrand = await brandRepository.ExistsAsync(brandId);
        if (!existingBrand) return false;

        var exists = await brandRepository.ExistsAsync(dto.Name, dto.BrandId);
        if (exists) throw new InvalidOperationException(LabelRegistry.Key.AlreadyExists);

        var result = await brandRepository.UpdateAsync(dto.ToEntity(brandId));

        if (result) await cache.RemoveAsync(GetCacheKeyItem(brandId), CacheKeyList);

        return result;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int brandId)
    {
        var brand = await brandRepository.FindAsync(brandId);
        if (brand == null) return false;

        var result = await brandRepository.DeleteAsync(brand);

        if (result) await cache.RemoveAsync(GetCacheKeyItem(brandId), CacheKeyList);

        return result;
    }
}
