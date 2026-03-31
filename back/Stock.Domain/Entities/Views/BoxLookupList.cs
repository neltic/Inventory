namespace Stock.Domain.Entities.Views;

public record BoxLookupList(
    int BoxId,
    string Name,
    DateTimeOffset UpdatedAt,
    int Indent
);