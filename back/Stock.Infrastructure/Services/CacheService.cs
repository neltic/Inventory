using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Stock.Application.Interfaces.Common;
using System.Text.Json;

namespace Stock.Infrastructure.Services;

public class CacheService(IDistributedCache cache, ILogger<CacheService> logger) : ICacheService
{
    private static bool _isRedisDown = false;
    private static long _lastRetryTicks = 0;
    private readonly TimeSpan _circuitBreakerTime = TimeSpan.FromSeconds(30);

    private readonly DistributedCacheEntryOptions _defaultOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(8),
        SlidingExpiration = TimeSpan.FromHours(1)
    };

    public async Task<T?> GetAsync<T>(string key)
    {
        if (CheckIfCacheServiceIsDown()) return default;        

        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(500));
            var jsonData = await cache.GetStringAsync(key, cts.Token);
            MarkCacheServiceAsUp();
            return jsonData is null ? default : JsonSerializer.Deserialize<T>(jsonData);
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning("Redis GetAsync timeout para key: {Key}", key);
            MarkCacheServiceAsDown();
            return default;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Redis GetAsync failed for key: {Key}", key);
            MarkCacheServiceAsDown();
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null)
    {
        if (CheckIfCacheServiceIsDown()) return;

        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? _defaultOptions.AbsoluteExpirationRelativeToNow,
                SlidingExpiration = unusedExpireTime ?? _defaultOptions.SlidingExpiration
            };

            var jsonData = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(key, jsonData, options, cts.Token);
            MarkCacheServiceAsUp();
        }
        catch (OperationCanceledException)
        {
            logger.LogWarning("Redis SetAsync timeout para key: {Key}", key);
            MarkCacheServiceAsDown();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Redis SetAsync failed for key: {Key}", key);
            MarkCacheServiceAsDown();
        }
    }

    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory)
    {
        var result = await GetAsync<T>(key);
        if (result != null) return result;

        result = await factory();

        if (result != null)
        {
            await SetAsync(key, result);
        }

        return result;
    }

    public async Task RemoveAsync(params string[] keys)
    {
        if (CheckIfCacheServiceIsDown() || keys == null || keys.Length == 0) return;

        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1000));
            var tasks = keys?.Where(key => !string.IsNullOrWhiteSpace(key)).Select(key => cache.RemoveAsync(key, cts.Token));

            if (tasks != null && tasks.Any())
            {
                await Task.WhenAll(tasks);
            }
            MarkCacheServiceAsUp();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Redis RemoveAsync failed for keys: {Keys}", string.Join(", ", keys ?? []));
            MarkCacheServiceAsDown();
        }
    }

    private bool CheckIfCacheServiceIsDown()
    {
        if (_isRedisDown)
        {
            var lastRetry = new DateTime(Interlocked.Read(ref _lastRetryTicks));
            if (DateTime.UtcNow < lastRetry.Add(_circuitBreakerTime))
            {
                return true;
            }
        }
        return false;
    }

    private static void MarkCacheServiceAsDown()
    {
        _isRedisDown = true;
        Interlocked.Exchange(ref _lastRetryTicks, DateTime.UtcNow.Ticks);
    }

    private static void MarkCacheServiceAsUp()
    {
        _isRedisDown = false;
        Interlocked.Exchange(ref _lastRetryTicks, 0);
    }
}