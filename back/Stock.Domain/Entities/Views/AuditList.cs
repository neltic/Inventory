using static Stock.Foundation.Common.SystemRegistry;

namespace Stock.Domain.Entities.Views;

public record AuditList(
    Event EventId,
    string By,
    DateTimeOffset At
    );