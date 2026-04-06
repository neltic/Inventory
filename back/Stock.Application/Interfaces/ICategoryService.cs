using Stock.Application.DTOs;

namespace Stock.Application.Interfaces;

/// <summary>
/// Defines the contract for managing product categories.
/// </summary>
public interface ICategoryService
{
    /// <summary>
    /// Retrieves a specific category by its unique identifier.
    /// </summary>
    /// <param name="categoryId">The unique ID of the category.</param>
    /// <returns>A <see cref="CategoryDto"/> if found; otherwise, null.</returns>
    Task<CategoryDto?> GetByIdAsync(int categoryId);

    /// <summary>
    /// Retrieves all categories registered in the system, typically ordered by their display sequence.
    /// </summary>
    /// <returns>A collection of all categories.</returns>
    Task<IEnumerable<CategoryDto>> GetAllAsync();

    /// <summary>
    /// Creates a new category record.
    /// </summary>
    /// <param name="dto">The category data to be created.</param>
    /// <returns>The unique identifier of the newly created category.</returns>
    /// <exception cref="InvalidOperationException">Thrown if a category with the same name already exists.</exception>
    Task<int> CreateAsync(CategoryDto dto);

    /// <summary>
    /// Updates the details of an existing category.
    /// </summary>
    /// <param name="categoryId">The ID of the category to update.</param>
    /// <param name="dto">The updated category information.</param>
    /// <returns>True if the update was successful; otherwise, false.</returns>
    Task<bool> UpdateAsync(int categoryId, CategoryDto dto);

    /// <summary>
    /// Deletes a category from the system.
    /// </summary>
    /// <remarks>
    /// Deletion should be restricted if there are items or subcategories associated with this record.
    /// </remarks>
    /// <param name="categoryId">The ID of the category to remove.</param>
    /// <returns>True if the deletion was successful; otherwise, false.</returns>
    Task<bool> DeleteAsync(int categoryId);

    /// <summary>
    /// Updates the display sequence of a category and reshuffles the surrounding items.
    /// </summary>
    /// <remarks>
    /// This method is intended for drag-and-drop UI functionality, ensuring that all categories 
    /// maintain a consistent and unique order index.
    /// </remarks>
    /// <param name="categoryId">The ID of the category to reorder.</param>
    /// <param name="newOrder">The target position/index for the category.</param>
    /// <returns>True if the reordering was successful; otherwise, false.</returns>
    Task<bool> ReorderAsync(int categoryId, int newOrder);
}