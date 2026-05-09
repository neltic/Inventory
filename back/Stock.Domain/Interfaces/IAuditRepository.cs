using Stock.Domain.Entities.Views;
using static Stock.Foundation.Common.SystemRegistry;

namespace Stock.Domain.Interfaces;

/// <summary>
/// Provides access to read-only audit log views from the persistence layer.
/// </summary>
public interface IAuditRepository
{
    /// <summary>
    /// Retrieves a collection of audit entries filtered by a specific entity type and its unique record identifier.
    /// </summary>
    /// <param name="entityId">The system entity type (e.g., Box, Category, Brand) defined in the System Registry.</param>
    /// <param name="recordId">The unique identifier of the specific record within the given entity.</param>
    /// <returns>The task result contains an enumerable list of <see cref="AuditList"/> entries.</returns>
    Task<IEnumerable<AuditList>> GetAuditListByAsync(Entity entityId, string recordId);
}