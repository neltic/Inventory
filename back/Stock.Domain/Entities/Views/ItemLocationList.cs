namespace Stock.Domain.Entities.Views;

public record ItemLocationList(
    int BoxId,
    string Name,
    int BrandId,
    int Quantity
);