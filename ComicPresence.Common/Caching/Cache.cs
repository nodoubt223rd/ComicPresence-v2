using System;
using System.Runtime.Caching;

namespace ComicPresence.Common.Caching
{
    public sealed class Cache : CacheBase
    {
        private readonly MemoryCache cache = new MemoryCache(typeof(Cache).ToString());

        protected override bool TryFind<TResult>(string key, CachePolicy policy, out TResult value)
        {
            var item = this.cache.GetCacheItem(key);
            if (item != null)
            {
                value = (TResult)item.Value;
                return true;
            }

            value = default(TResult);
            return false;
        }

        protected override void Add<TResult>(string key, TResult value, CachePolicy policy)
        {
            var cacheItem = new CacheItem(key, value);
            var cachePolicy = new CacheItemPolicy();
            if (policy.RenewLeaseOnAccess)
            {
                cachePolicy.SlidingExpiration = policy.ExpiresAfter;
            }
            else
            {
                cachePolicy.AbsoluteExpiration = DateTimeOffset.UtcNow + policy.ExpiresAfter;
            }

            this.cache.Add(cacheItem, cachePolicy);
        }
    }
}
