using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EStart.Infrastructure.Caching
{
    /// <summary>
    /// [扩展类]缓存管理
    /// </summary>
    public static class CacheExtension
    {
        /// <summary>
        /// 获取缓存值，如果缓存不存在，则读取数据源数据并添加至缓存
        /// </summary>
        /// <typeparam name="T">缓存类型</typeparam>
        /// <param name="cacheManager">缓存管理对象</param>
        /// <param name="key">键</param>
        /// <param name="acquire">获取数据源的方法</param>
        /// <returns></returns>
        public static T Get<T>(this ICacheManager cacheManager, string key, Func<T> acquire)
        {
            return Get(cacheManager, key, 60, acquire);
        }
        /// <summary>
        /// 获取缓存值，如果缓存不存在，则读取数据源数据并添加至缓存
        /// </summary>
        /// <typeparam name="T">缓存类型</typeparam>
        /// <param name="cacheManager">缓存管理对象</param>
        /// <param name="key">键</param>
        /// <param name="cacheTime">缓存时间</param>
        /// <param name="acquire">获取数据源的方法</param>
        /// <returns></returns>
        public static T Get<T>(this ICacheManager cacheManager, string key, int cacheTime, Func<T> acquire)
        {
            if (cacheManager.IsSet(key))
            {
                return cacheManager.Get<T>(key);
            }

            var result = acquire();
            if (cacheTime > 0)
            {
                cacheManager.GetOrCreate<T>(key, entry => {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(cacheTime);
                    return result;
                });
            }
            return result;
        }
    }
}
