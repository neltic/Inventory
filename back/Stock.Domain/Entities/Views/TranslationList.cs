namespace Stock.Domain.Entities.Views;

public record TranslationList(
    string LanguageCode,
    string Context,
    string LabelKey,
    string Text
    );
