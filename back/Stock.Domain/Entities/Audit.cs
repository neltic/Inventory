using static Stock.Foundation.Common.SystemRegistry;

namespace Stock.Domain.Entities;

/// <summary>
/// Represents a centralized audit log entry for tracking data changes and system activity.
/// </summary>
public class Audit
{
    /// <summary>
    /// Gets or sets the unique identifier for the audit record.
    /// </summary>
    public long AuditId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the entity (table) being audited.
    /// Mapped to <see cref="Entity"/>.
    /// </summary>
    public Entity EntityId { get; set; }

    /// <summary>
    /// Gets or sets the primary key value of the specific record being audited.
    /// </summary>
    public string RecordId { get; set; } = null!;

    /// <summary>
    /// Gets or sets the type of operation performed (e.g., Create, Update, Delete).
    /// Mapped to <see cref="Event"/>.
    /// </summary>
    public Event EventId { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the user or system process that performed the action.
    /// Typically contains the username or ClientId from the identity provider.
    /// </summary>
    public string By { get; set; } = null!;

    /// <summary>
    /// Gets or sets the exact date and time when the audit event occurred.
    /// </summary>
    public DateTimeOffset At { get; set; }

    /// <summary>
    /// Gets or sets a JSON-serialized string containing the audit metadata, 
    /// including user snapshots, original values, and modified values.
    /// </summary>
    public string Context { get; set; } = null!;
}