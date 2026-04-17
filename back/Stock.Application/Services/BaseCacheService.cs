namespace Stock.Application.Services;

public abstract class BaseCacheService(string type, string entityName)
{
    protected readonly string _entityName = entityName.ToLower();
    protected readonly string _typeName = type.ToLower();

    protected string CacheKeyList => $"{_typeName}:{_entityName}:all";

    protected string GetCacheKeyItem(object id) => $"{_typeName}:{_entityName}:entity:{id}";
}