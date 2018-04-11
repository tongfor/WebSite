using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EStart.Infrastructure.Caching
{
    public interface ICacheManager
    {
        /// <summary>
        /// 将数据添加至缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="data">数据</param>
        TItem Set<TItem>(object key, TItem data);
        TItem GetOrCreate<TItem>(object key,Func<ICacheEntry,TItem> factory);
        Task<TItem> GetOrCreateAsync<TItem>(object key, Func<ICacheEntry, Task<TItem>> factory);
        bool TryGetValue<TItem>(object key, out TItem val);
        /// <summary>
        /// 获取指定键是否已存在缓存
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns></returns>
        bool IsSet(object key);
        /// <summary>
        /// 获取缓存内容
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">Key</param>
        /// <returns></returns>
        T Get<T>(object key);
        /// <summary>
        /// 移除一个缓存
        /// </summary>
        /// <param name="key">Key</param>
        void Remove(object key);
    }
}
