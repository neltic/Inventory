namespace Stock.Domain.Entities.Views;

public record BoxTransferList(
    int? BoxId,
    string Name,
    DateTimeOffset UpdatedAt,
    int Indent,
    bool IsSelectable
);