using Stock.Application.Interfaces;
using Stock.Application.Interfaces.Common;

namespace Stock.Api.Utils;

public class LanguageDetectionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context,
                                  IGlobalizationService globalizationService,
                                  ITranslationStorage translationStorage)
    {
        var requestedLanguage = context.Request.Headers.AcceptLanguage.ToString();

        int commaIndex = requestedLanguage.IndexOf(',');

        var language = string.IsNullOrEmpty(requestedLanguage)
               ? translationStorage.DefaultLanguage
               : commaIndex != -1
                    ? requestedLanguage[..commaIndex]
                    : requestedLanguage;

        globalizationService.CurrentLanguage = language;

        await next(context);
    }
}
