using Stock.Application.Interfaces.Common;

namespace Stock.Application.Services;

public abstract class BaseCacheService(string entityName)
{
    protected readonly string _entityName = entityName.ToLower();

    protected string CacheKeyList => $"catalog:{_entityName}:all";

    protected string GetCacheKeyItem(int id) => $"catalog:{_entityName}:item:{id}";
}