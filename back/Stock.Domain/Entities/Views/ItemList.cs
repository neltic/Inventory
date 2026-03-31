namespace Stock.Domain.Entities.Views;

public record ItemList(
    int ItemId,
    string Name,
    int CategoryId,
    DateTimeOffset UpdatedAt,
    bool HasStock
    );
