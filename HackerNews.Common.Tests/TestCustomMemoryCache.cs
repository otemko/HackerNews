using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

public class TestCustomMemoryCache : IMemoryCache
{
    private readonly Dictionary<object, object> _cache = new Dictionary<object, object>();

    public ICacheEntry CreateEntry(object key)
    {
        var entry = new CustomCacheEntry(key);
        _cache[key] = entry.Value;
        return entry;
    }

    public void Dispose()
    {
        _cache.Clear();
    }

    public void Remove(object key)
    {
        _cache.Remove(key);
    }

    public bool TryGetValue(object key, out object value)
    {
        return _cache.TryGetValue(key, out value);
    }

    private class CustomCacheEntry : ICacheEntry
    {
        public CustomCacheEntry(object key)
        {
            Key = key;
        }

        public void Dispose()
        {
        }

        public object Key { get; }
        public object Value { get; set; }
        public DateTimeOffset? AbsoluteExpiration { get; set; }
        public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
        public TimeSpan? SlidingExpiration { get; set; }
        public IList<IChangeToken> ExpirationTokens { get; } = new List<IChangeToken>();
        public IList<PostEvictionCallbackRegistration> PostEvictionCallbacks { get; } = new List<PostEvictionCallbackRegistration>();
        public CacheItemPriority Priority { get; set; }
        public long? Size { get; set; }

        IList<IChangeToken> ICacheEntry.ExpirationTokens => throw new NotImplementedException();
    }
}