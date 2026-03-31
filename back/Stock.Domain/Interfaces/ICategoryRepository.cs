using Stock.Domain.Entities;

namespace Stock.Domain.Interfaces;

/// <summary>
/// Repository interface for direct data access and management of Category entities.
/// </summary>
public interface ICategoryRepository
{
    /// <summary>
    /// Checks if a category with the specified identifier exists in the database.
    /// </summary>
    /// <param name="categoryId">The unique ID of the category.</param>
    /// <returns>True if the category exists; otherwise, false.</returns>
    Task<bool> ExistsAsync(int categoryId);

    /// <summary>
    /// Checks if a category name already exists in the database.
    /// </summary>
    /// <remarks>
    /// Used to enforce unique naming conventions for categories across the system.
    /// </remarks>
    /// <param name="name">The name to check.</param>
    /// <param name="excludeId">An optional ID to exclude from the check (useful for updates).</param>
    /// <returns>True if a duplicate name is found; otherwise, false.</returns>
    Task<bool> ExistsAsync(string name, int? excludeId = null);

    /// <summary>
    /// Retrieves a specific category by its unique identifier.
    /// </summary>
    /// <param name="categoryId">The ID of the category.</param>
    /// <returns>The <see cref="Category"/> entity if found; otherwise, null.</returns>
    Task<Category?> GetByIdAsync(int categoryId);

    /// <summary>
    /// Retrieves all category records, typically sorted by their defined display order.
    /// </summary>
    /// <returns>A collection of all <see cref="Category"/> entities.</returns>
    Task<IEnumerable<Category>> GetAllAsync();

    /// <summary>
    /// Persists a new category record to the database.
    /// </summary>
    /// <param name="category">The category entity to add.</param>
    /// <returns>The newly assigned unique identifier for the category.</returns>
    Task<int> AddAsync(Category category);

    /// <summary>
    /// Updates an existing category's information in the database.
    /// </summary>
    /// <param name="category">The category entity with updated values.</param>
    /// <returns>True if the update was successful; otherwise, false.</returns>
    Task<bool> UpdateAsync(Category category);

    /// <summary>
    /// Removes a category record from the database by its identifier.
    /// </summary>
    /// <param name="categoryId">The ID of the category to remove.</param>
    /// <returns>True if the deletion was successful; otherwise, false.</returns>
    Task<bool> DeleteAsync(int categoryId);

    /// <summary>
    /// Executes the database-level logic to update the display sequence of a category.
    /// </summary>
    /// <remarks>
    /// This may involve a stored procedure or a batch update to reshuffle the 'Order' 
    /// values of sibling categories.
    /// </remarks>
    /// <param name="categoryId">The ID of the category to move.</param>
    /// <param name="newOrder">The target position/index for the category.</param>
    /// <returns>True if the reordering operation succeeded; otherwise, false.</returns>
    Task<bool> ReorderAsync(int categoryId, int newOrder);
}