using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.DTOs;
using Stock.Application.Interfaces;
using Stock.Foundation.Common;
using static Stock.Foundation.Common.SystemRegistry;

namespace Stock.Api.Controllers;

/// <summary>
/// Controller for accessing system audit logs and activity history.
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AuditController(IAuditService auditService) : ControllerBase
{
    /// <summary>
    /// Retrieves the audit history for a specific record.
    /// </summary>
    /// <remarks>
    /// Returns a list of events associated with an entity and record ID, 
    /// including who performed the action and when it occurred.
    /// </remarks>
    /// <param name="entityId">The type of entity (e.g., Box, Category).</param>
    /// <param name="recordId">The unique identifier of the specific record.</param>
    /// <returns>A collection of audit trail entries.</returns>
    /// <response code="200">Returns the list of audit entries.</response>
    [HttpGet("{entityId}/{recordId}")]
    [Authorize(Roles = RoleRealm.Viewer)]
    [ProducesResponseType(typeof(IEnumerable<AuditListDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHistory(Entity entityId, string recordId)
    {
        var history = await auditService.GetAuditHistoryAsync(entityId, recordId);
    
        return Ok(history);
    }
}