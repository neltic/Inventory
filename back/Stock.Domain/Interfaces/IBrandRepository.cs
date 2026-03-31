using Stock.Domain.Entities;

namespace Stock.Domain.Interfaces;

/// <summary>
/// Repository interface for direct data access and persistence operations for Brand entities.
/// </summary>
public interface IBrandRepository
{
    /// <summary>
    /// Finds a Brand entity by its unique identifier.
    /// </summary>
    /// <param name="BrandId">The unique ID of the brand.</param>
    /// <returns>The <see cref="Brand"/> entity if found; otherwise, null.</returns>
    Task<Brand?> FindAsync(int BrandId);

    /// <summary>
    /// Checks if a brand with the specified ID exists in the database.
    /// </summary>
    /// <param name="BrandId">The ID to check.</param>
    /// <returns>True if the brand exists; otherwise, false.</returns>
    Task<bool> ExistsAsync(int BrandId);

    /// <summary>
    /// Checks if a brand name already exists in the database.
    /// </summary>
    /// <remarks>
    /// Used for validation to ensure brand names are unique across the system.
    /// </remarks>
    /// <param name="name">The name to check.</param>
    /// <param name="excludeId">An optional ID to exclude (useful during update operations).</param>
    /// <returns>True if a duplicate name is found; otherwise, false.</returns>
    Task<bool> ExistsAsync(string name, int? excludeId = null);

    /// <summary>
    /// Retrieves a brand record by its identifier.
    /// </summary>
    /// <param name="BrandId">The ID of the brand.</param>
    /// <returns>The found <see cref="Brand"/>; otherwise, null.</returns>
    Task<Brand?> GetByIdAsync(int BrandId);

    /// <summary>
    /// Retrieves all brand records currently stored in the database.
    /// </summary>
    /// <returns>A collection of all <see cref="Brand"/> entities.</returns>
    Task<IEnumerable<Brand>> GetAllAsync();

    /// <summary>
    /// Persists a new brand entity to the database.
    /// </summary>
    /// <param name="Brand">The brand entity to add.</param>
    /// <returns>The newly assigned unique identifier for the brand.</returns>
    Task<int> AddAsync(Brand Brand);

    /// <summary>
    /// Updates an existing brand record in the database.
    /// </summary>
    /// <param name="Brand">The brand entity with updated values.</param>
    /// <returns>True if the database update was successful; otherwise, false.</returns>
    Task<bool> UpdateAsync(Brand Brand);

    /// <summary>
    /// Removes a brand record from the database.
    /// </summary>
    /// <param name="Brand">The brand entity to remove.</param>
    /// <returns>True if the deletion was successful; otherwise, false.</returns>
    Task<bool> DeleteAsync(Brand Brand);
}