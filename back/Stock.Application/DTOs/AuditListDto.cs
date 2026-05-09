using static Stock.Foundation.Common.SystemRegistry;

namespace Stock.Application.DTOs;

/// <summary>
/// Data transfer object for displaying audit trail entries in lists or activity logs.
/// </summary>
/// <param name="EventId">The unique identifier or type of the logged event.</param>
/// <param name="By">The identity or username of the actor who performed the action.</param>
/// <param name="At">The precise timestamp when the event occurred, including time zone offset.</param>
public record AuditListDto(
    Event EventId,
    string By,
    DateTimeOffset At
);