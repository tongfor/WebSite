using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EStart.Infrastructure.Caching
{
    public class MemoryCacheAdapter : ICacheManager
    {
        private readonly IMemoryCache _cache;
        public MemoryCacheAdapter(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T Get<T>(object key)
        {
            return _cache.Get<T>(key);
        }

        public TItem GetOrCreate<TItem>(object key, Func<ICacheEntry, TItem> factory)
        {
            return _cache.GetOrCreate(key, factory);
        }

        public async Task<TItem> GetOrCreateAsync<TItem>(object key, Func<ICacheEntry, Task<TItem>> factory)
        {
            return await _cache.GetOrCreateAsync(key, factory);
        }

        public bool IsSet(object key)
        {
            return _cache.Get(key) != null;
        }

        public void Remove(object key)
        {
            _cache.Remove(key);
        }

        public TItem Set<TItem>(object key, TItem data)
        {
            return _cache.Set(key, data);
        }

        public bool TryGetValue<TItem>(object key, out TItem val)
        {
            return _cache.TryGetValue(key, out val);
        }
    }
}
