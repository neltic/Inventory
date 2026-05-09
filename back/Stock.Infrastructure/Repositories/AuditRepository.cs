using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities.Views;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Persistence;
using static Stock.Foundation.Common.SystemRegistry;

namespace Stock.Infrastructure.Repositories;

public class AuditRepository(StockDbContext context) : IAuditRepository
{
    public async Task<IEnumerable<AuditList>> GetAuditListByAsync(Entity entityId, string recordId)
    {
        return await context.Audits
            .AsNoTracking()
            .OrderBy(a => a.At)
            .Where(a => a.EntityId == entityId && a.RecordId == recordId)
            .Select(a => new AuditList(
                a.EventId,
                a.By,
                a.At
            ))
            .ToListAsync();
    }
}
