using Stock.Application.DTOs;
using Stock.Application.Interfaces;
using Stock.Application.Mappings;
using Stock.Domain.Interfaces;

namespace Stock.Application.Services;

public class BrandService(IBrandRepository BrandRepository) : IBrandService
{
    /// <inheritdoc />
    public async Task<BrandDto?> GetByIdAsync(int BrandId)
    {
        if (BrandId > 0)
        {
            var Brand = await BrandRepository.GetByIdAsync(BrandId);

            return Brand?.ToDto();
        }
        return null;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BrandDto>> GetAllAsync()
    {
        var results = await BrandRepository.GetAllAsync();

        return results.ToDtoList();
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(BrandDto dto)
    {
        var exists = await BrandRepository.ExistsAsync(dto.Name);

        if (exists)
        {
            throw new InvalidOperationException("A Brand with this name already exists.");
        }

        var Brand = dto.ToEntity(0);

        return await BrandRepository.AddAsync(Brand);
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(int BrandId, BrandDto dto)
    {
        var existingBrand = await BrandRepository.ExistsAsync(BrandId);

        if (!existingBrand)
        {
            return false;
        }

        var exists = await BrandRepository.ExistsAsync(dto.Name, dto.BrandId);

        if (exists)
        {
            throw new InvalidOperationException("A Brand with this name already exists.");
        }

        return await BrandRepository.UpdateAsync(dto.ToEntity(BrandId));
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int BrandId)
    {
        var Brand = await BrandRepository.FindAsync(BrandId);
        if (Brand == null) return false;

        return await BrandRepository.DeleteAsync(Brand);
    }
}
