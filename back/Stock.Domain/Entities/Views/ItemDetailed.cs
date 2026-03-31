namespace Stock.Domain.Entities.Views;

public record ItemDetailed(
    int ItemId,
    string Name,
    string Notes,
    int CategoryId,
    string? InBox,    
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);