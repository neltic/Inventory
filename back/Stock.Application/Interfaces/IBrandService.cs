using Stock.Application.DTOs;

namespace Stock.Application.Interfaces;

/// <summary>
/// Defines the contract for managing product brands within the inventory system.
/// </summary>
public interface IBrandService
{
    /// <summary>
    /// Retrieves a specific brand by its unique identifier.
    /// </summary>
    /// <param name="brandId">The unique ID of the brand.</param>
    /// <returns>A <see cref="BrandDto"/> if found; otherwise, null.</returns>
    Task<BrandDto?> GetByIdAsync(int brandId);

    /// <summary>
    /// Retrieves all brands registered in the system.
    /// </summary>
    /// <returns>A collection of all brands.</returns>
    Task<IEnumerable<BrandDto>> GetAllAsync();

    /// <summary>
    /// Creates a new brand record.
    /// </summary>
    /// <param name="dto">The brand data to be created.</param>
    /// <returns>The unique identifier of the newly created brand.</returns>
    /// <exception cref="InvalidOperationException">Thrown if a brand with the same name already exists.</exception>
    Task<int> CreateAsync(BrandDto dto);

    /// <summary>
    /// Updates the details of an existing brand.
    /// </summary>
    /// <param name="brandId">The ID of the brand to update.</param>
    /// <param name="dto">The updated brand information.</param>
    /// <returns>True if the update was successful; otherwise, false.</returns>
    Task<bool> UpdateAsync(int brandId, BrandDto dto);

    /// <summary>
    /// Deletes a brand from the system.
    /// </summary>
    /// <remarks>
    /// Implementation should ensure that no items are currently associated with this brand before deletion.
    /// </remarks>
    /// <param name="brandId">The ID of the brand to remove.</param>
    /// <returns>True if the deletion was successful; otherwise, false.</returns>
    Task<bool> DeleteAsync(int brandId);
}