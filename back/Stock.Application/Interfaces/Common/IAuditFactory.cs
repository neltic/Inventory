using Stock.Application.Common;
using Stock.Domain.Entities;

namespace Stock.Application.Interfaces.Common;

public interface IAuditFactory
{
    Audit Create(AuditRequest request);

    IEnumerable<Audit> Create(IEnumerable<AuditRequest> requestList);
}