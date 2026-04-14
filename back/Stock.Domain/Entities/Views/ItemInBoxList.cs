namespace Stock.Domain.Entities.Views;

public record ItemInBoxList(
    int ItemId,
    string Name,
    int BrandId,
    int CategoryId,
    int Quantity,
    DateTimeOffset UpdatedAt
    );
