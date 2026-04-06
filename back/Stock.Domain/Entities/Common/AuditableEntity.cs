namespace Stock.Domain.Entities.Common;

/// <summary>
/// Abstract base class that provides automatic auditing metadata for domain entities.
/// </summary>
/// <remarks>
/// This class ensures that any entity inheriting from it will track its lifecycle 
/// from creation to the last modification. In the infrastructure layer, 
/// these fields are typically populated automatically by the Database Context.
/// </remarks>
public abstract class AuditableEntity
{
    /// <summary>
    /// Gets or sets the exact date and time when the entity was first persisted to the database.
    /// </summary>
    /// <value>A <see cref="DateTimeOffset"/> representing the creation moment in UTC.</value>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time of the last successful update to any property of this entity.
    /// </summary>
    /// <value>A <see cref="DateTimeOffset"/> representing the last update moment in UTC.</value>
    public DateTimeOffset UpdatedAt { get; set; }
}