using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Stock.Application.Interfaces.Common;
using System.Text.Json;

namespace Stock.Infrastructure.Services;

public class CacheService(IDistributedCache cache, ILogger<CacheService> logger) : ICacheService
{
    private readonly DistributedCacheEntryOptions _defaultOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(8),
        SlidingExpiration = TimeSpan.FromHours(1)
    };

    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            var jsonData = await cache.GetStringAsync(key);
            return jsonData is null ? default : JsonSerializer.Deserialize<T>(jsonData);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Redis GetAsync failed for key: {Key}", key);
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null)
    {
        try
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? _defaultOptions.AbsoluteExpirationRelativeToNow,
                SlidingExpiration = unusedExpireTime ?? _defaultOptions.SlidingExpiration
            };

            var jsonData = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(key, jsonData, options);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Redis SetAsync failed for key: {Key}", key);
        }
    }

    public async Task RemoveAsync(params string[] keys)
    {
        try
        {
            var tasks = keys?.Where(key => !string.IsNullOrWhiteSpace(key)).Select(key => cache.RemoveAsync(key));

            if (tasks != null && tasks.Any())
            {
                await Task.WhenAll(tasks);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Redis RemoveAsync failed for keys: {Keys}", string.Join(", ", keys ?? []));
        }
    }
}