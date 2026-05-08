namespace Stock.Domain.Entities.Views;

public record BoxTransferList(
    int? BoxId,
    string Name,
    DateTimeOffset ImageAt,
    int Indent,
    bool IsSelectable
);