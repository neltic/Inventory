using Stock.Application.DTOs;
using Stock.Application.Interfaces;
using Stock.Application.Mappings;
using Stock.Domain.Interfaces;

namespace Stock.Application.Services;

public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    /// <inheritdoc />
    public async Task<CategoryDto?> GetByIdAsync(int categoryId)
    {
        if (categoryId > 0)
        {
            var category = await categoryRepository.GetByIdAsync(categoryId);

            return category?.ToDto();
        }
        return null;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        var results = await categoryRepository.GetAllAsync();

        return results.ToDtoList();
    }

    /// <inheritdoc />
    public async Task<int> CreateAsync(CategoryDto dto)
    {
        var exists = await categoryRepository.ExistsAsync(dto.Name);

        if (exists)
        {
            throw new InvalidOperationException("A category with this name already exists.");
        }

        var category = dto.ToEntity(0);

        return await categoryRepository.AddAsync(category);
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(int categoryId, CategoryDto dto)
    {
        var existingCategory = await categoryRepository.ExistsAsync(categoryId);

        if (!existingCategory)
        {
            return false;
        }

        var exists = await categoryRepository.ExistsAsync(dto.Name, dto.CategoryId);

        if (exists)
        {
            throw new InvalidOperationException("A category with this name already exists.");
        }

        return await categoryRepository.UpdateAsync(dto.ToEntity(categoryId));
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int categoryId)
    {        
        return await categoryRepository.DeleteAsync(categoryId);
    }

    /// <inheritdoc />
    public async Task<bool> ReorderAsync(int categoryId, int newOrder)
    {
        return await categoryRepository.ReorderAsync(categoryId, newOrder);
    }
}
