using Stock.Application.Common;
using Stock.Application.Interfaces.Common;
using Stock.Domain.Entities;
using System.Text.Json;

namespace Stock.Infrastructure.Services;

public class AuditFactory(ICurrentUserService currentUserService) : IAuditFactory
{
    public Audit Create(AuditRequest request)
    {
        return Create(request, currentUserService.GetInfo());
    }

    public IEnumerable<Audit> Create(IEnumerable<AuditRequest> requestList)
    {
        var userInfo = currentUserService.GetInfo();
        return requestList.Select(request => Create(request, userInfo));
    }

    private static Audit Create(AuditRequest request, CurrentUserInfo userInfo)
    {
        var context = new
        {
            User = userInfo,
            Changes = new { Old = request.OldValues, New = request.NewValues }
        };

        return new Audit
        {
            EntityId = request.EntityId,
            EventId = request.EventId,
            RecordId = request.RecordId,
            By = userInfo.Username,
            At = DateTimeOffset.UtcNow,
            Context = JsonSerializer.Serialize(context)
        };
    }
}
