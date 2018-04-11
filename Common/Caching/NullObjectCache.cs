using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace EStart.Infrastructure.Caching
{
    public class NullObjectCache : ICacheManager
    {
        public T Get<T>(object key)
        {
            return default(T);
        }

        public TItem GetOrCreate<TItem>(object key, Func<ICacheEntry, TItem> factory)
        {
            return default(TItem);
            //throw new NotImplementedException();
        }

        public Task<TItem> GetOrCreateAsync<TItem>(object key, Func<ICacheEntry, Task<TItem>> factory)
        {
            return null;
            //throw new NotImplementedException();
        }

        public bool IsSet(object key)
        {
            return false;
            //throw new NotImplementedException();
        }

        public void Remove(object key)
        {
            //throw new NotImplementedException();
        }

        public TItem Set<TItem>(object key, TItem data)
        {
            return default(TItem);
            //throw new NotImplementedException();
        }

        public bool TryGetValue<TItem>(object key, out TItem val)
        {
            val = default(TItem);
            return false;
            //throw new NotImplementedException();
        }
    }
}
