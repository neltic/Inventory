using Stock.Application.DTOs;
using static Stock.Foundation.Common.SystemRegistry;

namespace Stock.Application.Interfaces;

/// <summary>
/// Defines the application service for managing and retrieving audit-related data.
/// </summary>
public interface IAuditService
{
    /// <summary>
    /// Fetches the audit history for a specific record and projects it into a list of DTOs.
    /// </summary>
    /// <param name="entityId">The type of entity to audit (from <see cref="Entity"/>).</param>
    /// <param name="recordId">The unique identifier of the specific record.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing a collection 
    /// of <see cref="AuditListDto"/> objects formatted for the UI.
    /// </returns>
    /// <remarks>
    /// This method is typically used to populate "History" in the user interface.
    /// </remarks>
    Task<IEnumerable<AuditListDto>> GetAuditHistoryAsync(Entity entityId, string recordId);
}