using System.Text.Json.Serialization;
using static Stock.Foundation.Common.SystemRegistry;

namespace Stock.Application.Common;

public class AuditRequest
{
    public Entity EntityId { get; set; }
    public Event EventId { get; set; }
    public string RecordId { get; set; } = null!;
    public Dictionary<string, object?> OldValues { get; set; } = [];
    public Dictionary<string, object?> NewValues { get; set; } = [];    
}