namespace Stock.Application.Common;

public record ApiError(
    int StatusCode,
    string Message,
    string? Detail = null
);