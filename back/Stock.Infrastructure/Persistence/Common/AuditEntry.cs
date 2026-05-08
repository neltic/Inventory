using Microsoft.EntityFrameworkCore.ChangeTracking;
using Stock.Application.Common;
using Stock.Domain.Entities;
using System.Text.Json;
using static Stock.Foundation.Common.SystemRegistry;

namespace Stock.Infrastructure.Persistence.Common;

public class AuditEntry(EntityEntry entry)
{
    public EntityEntry Entry { get; } = entry;
    public Entity EntityId { get; set; }
    public Event EventId { get; set; }
    public string RecordId { get; set; } = null!;
    public string By { get; set; } = null!;
    public DateTimeOffset At { get; set; }
    public CurrentUserInfo? UserSnapshot { get; set; }
    public Dictionary<string, object?> OldValues { get; } = [];
    public Dictionary<string, object?> NewValues { get; } = [];
    public List<PropertyEntry> TemporaryProperties { get; } = [];

    public Audit ToAuditEntity()
    {
        var context = new
        {
            User = UserSnapshot,
            Changes = new
            {
                Old = OldValues.Count > 0 ? OldValues : null,
                New = NewValues.Count > 0 ? NewValues : null
            }
        };

        return new Audit
        {
            EntityId = EntityId,
            EventId = EventId,
            RecordId = RecordId,
            By = By,
            At = At,
            Context = JsonSerializer.Serialize(context)
        };
    }
}