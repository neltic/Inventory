namespace Stock.Domain.Entities.Views;

public record ItemDetailed(
    int ItemId,
    string Name,
    string Notes,
    int CategoryId,      
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt
);