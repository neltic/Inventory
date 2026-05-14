namespace Stock.Application.Interfaces.Common;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null);
    Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory);
    Task RemoveAsync(params string[] keys);
}
