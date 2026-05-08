namespace Stock.Domain.Entities.Views;

public record BoxLookupList(
    int BoxId,
    string Name,
    DateTimeOffset ImageAt,
    int Indent
);