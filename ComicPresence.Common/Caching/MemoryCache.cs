using System;
using System.Threading.Tasks;

namespace ComicPresence.Common.Caching
{
    /// <summary>
    /// Thread-safe
    /// </summary>
    /// <typeparam name="TRegion"></typeparam>
    internal class MemoryCache<TRegion> : ICache<TRegion>
    {
        #region Private Fields

        private System.Runtime.Caching.MemoryCache _cache;
        private const string cCacheName = "AppCache";
        private const string cDefaultKey = "defaultKey";

        #endregion

        #region Public Properties

        public CacheStats<TRegion> CacheStats { get; protected set; }

        #endregion

        #region Constructors

        internal MemoryCache()
        {
            _cache = new System.Runtime.Caching.MemoryCache(cCacheName);
            CacheStats = new CacheStats<TRegion>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets an item from the cache
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="region">Region</param>
        /// <returns>The item if it's in cache. default(TResult) otherwise.</returns>
        public TResult Get<TResult>(TRegion region, string key)
        {
            bool found;
            return GetInternal<TResult>(region, key, out found);
        }

        /// <summary>
        /// Gets data from the cache. If that data is not available, it will be retrieved and cached. 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <param name="expiration">Item expires after this amount of time</param>
        /// <param name="dataRetrievalFunc">Function for retrieving data</param>
        /// <returns></returns>
        public TResult Get<TResult>(TRegion region, string key, TimeSpan expiration, Func<TResult> dataRetrievalFunc)
        {
            bool found;
            TResult data = GetInternal<TResult>(region, key, out found);

            if (!found)
            {
                data = dataRetrievalFunc();

                Put(region, key, data, expiration);
            }

            return data;
        }

        /// <summary>
        /// Gets an item from the cache
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="region">Region</param>
        /// <returns>The item if it's in cache. default(TResult) otherwise.</returns>
        public async Task<TResult> GetAsync<TResult>(TRegion region, string key)
        {
            key = CreateInternalKey(region, key);

            bool found;
            TResult data = GetInternal<TResult>(region, key, out found);

            return await Task.FromResult(data).ConfigureAwait(false);
        }

        /// <summary>
        /// Async version of Get&lt;TResult&gt;
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <param name="expiration"></param>
        /// <param name="dataRetrievalFunc"></param>
        /// <returns></returns>
        public async Task<TResult> GetAsync<TResult>(TRegion region, string key, TimeSpan expiration, Func<Task<TResult>> dataRetrievalFunc)
        {
            key = CreateInternalKey(region, key);

            bool found;
            TResult data = GetInternal<TResult>(region, key, out found);

            if (!found)
            {
                data = await dataRetrievalFunc().ConfigureAwait(false);

                Put(region, key, data, expiration);
            }

            return data;
        }

        public async Task GetParallelAsync(params CacheGetCommand<object, TRegion>[] commands)
        {
            await Task.FromResult(0).ConfigureAwait(false); // suppress warning
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes an item from the cache
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="region">Region</param>
        public void Remove(TRegion region, string key)
        {
            key = CreateInternalKey(region, key);

            _cache.Remove(key);
        }

        public void RemoveAll()
        {
            _cache = new System.Runtime.Caching.MemoryCache(cCacheName);
        }

        public Task RemoveAsync(TRegion region, string key)
        {
            key = CreateInternalKey(region, key);

            _cache.Remove(key);

            return Task.Factory.StartNew(() => { });
        }

        /// <summary>
        /// Add/overwrite an item in cache
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="region">Region</param>
        /// <param name="expiration">Expiration</param>
        public void Put(TRegion region, string key, object value, TimeSpan expiration)
        {
            key = CreateInternalKey(region, key);

            _cache.Set(new System.Runtime.Caching.CacheItem(key, value),
                new System.Runtime.Caching.CacheItemPolicy()
                {
                    AbsoluteExpiration = new DateTimeOffset(DateTime.UtcNow.AddTicks(expiration.Ticks))
                });
        }

        public async Task PutAsync(TRegion region, string key, object value, TimeSpan expiration)
        {
            await Task.Factory.StartNew(() => Put(region, key, value, expiration)).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates an internal key
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string CreateInternalKey(TRegion region, string key)
        {
            if (key == null)
                key = cDefaultKey;
            return String.Format("{0}_{1}", region.ToString(), key);
        }

        #endregion

        #region Non-public Methods

        /// <summary>
        /// Gets an item from the cache
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="key">If null, uses a default key</param>
        /// <param name="region"></param>
        /// <param name="found">Whether the item was found in cache</param>
        /// <returns></returns>
        protected TResult GetInternal<TResult>(TRegion region, string key, out bool found)
        {
            key = CreateInternalKey(region, key);

            System.Runtime.Caching.CacheItem item = _cache.GetCacheItem(key);
            if (item == null)
            {
                found = false;
                CacheStats.IncrementMisses(region);
                return default(TResult);
            }

            found = true;
            CacheStats.IncrementHits(region);
            return (TResult)item.Value;
        }

        #endregion
    }
}
