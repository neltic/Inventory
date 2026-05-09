using Stock.Application.DTOs;
using Stock.Application.Interfaces;
using Stock.Application.Mappings;
using Stock.Domain.Interfaces;
using static Stock.Foundation.Common.SystemRegistry;

namespace Stock.Application.Services;

public class AuditService(IAuditRepository auditRepository) : IAuditService
{

    /// <inheritdoc />
    public async Task<IEnumerable<AuditListDto>> GetAuditHistoryAsync(Entity entityId, string recordId)
    {
        var auditEntities = await auditRepository.GetAuditListByAsync(entityId, recordId);
        
        return auditEntities.ToDtoList();
    }
}