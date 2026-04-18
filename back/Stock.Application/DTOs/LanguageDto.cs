namespace Stock.Application.DTOs;

public record LanguageDto(
    string LanguageCode,
    string Name,
    bool IsDefault
    );
