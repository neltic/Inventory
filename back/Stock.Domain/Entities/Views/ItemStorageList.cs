namespace Stock.Domain.Entities.Views;

public record ItemStorageList(
    int BoxId,
    string Name,
    int BrandId,
    DateTimeOffset UpdatedAt,
    int Quantity,
    bool Expires,
    DateOnly? ExpiresOn
    );
