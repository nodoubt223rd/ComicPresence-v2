using System;
using System.Threading.Tasks;

namespace ComicPresence.Common.Caching
{
    /// <summary>
    /// Thread-safe
    /// </summary>
    /// <typeparam name="TRegion">Region type. Used as a partial namespace/key.</typeparam>
    public interface ICache<TRegion>
    {
        CacheStats<TRegion> CacheStats { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="region"></param>
        /// <param name="key">If null, a default key will be used</param>
        /// <returns>default(TResult) if not found</returns>
        TResult Get<TResult>(TRegion region, string key);

        /// <summary>
        /// Gets data from the cache. If that data is not available, it will be retrieved and cached. 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <param name="expiration">Item expires after this amount of time</param>
        /// <param name="dataRetrievalFunc">Function used to retrieve data</param>
        /// <returns></returns>
        TResult Get<TResult>(TRegion region, string key, TimeSpan expiration, Func<TResult> dataRetrievalFunc);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="region"></param>
        /// <param name="key">If null, a default key will be used</param>
        /// <returns>default(TResult) if not found</returns>
        Task<TResult> GetAsync<TResult>(TRegion region, string key);

        /// <summary>
        /// Async version of Get&lt;TResult&gt;
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <param name="expiration"></param>
        /// <param name="dataRetrievalFunc"></param>
        /// <returns></returns>
        Task<TResult> GetAsync<TResult>(TRegion region, string key, TimeSpan expiration, Func<Task<TResult>> dataRetrievalFunc);

        Task GetParallelAsync(params CacheGetCommand<object, TRegion>[] commands);

        /// <summary>
        /// Does nothing if item is not in cache
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        void Remove(TRegion region, string key);

        /// <summary>
        /// Clears the cache
        /// </summary>
        void RemoveAll();

        /// <summary>
        /// Async version of Remove
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        Task RemoveAsync(TRegion region, string key);

        /// <summary>
        /// Adds or overwrites data in cache
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiration">Absolute/fixed expiration</param>
        void Put(TRegion region, string key, object value, TimeSpan expiration);

        /// <summary>
        /// Async version of put
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiration"></param>
        /// <returns></returns>
        Task PutAsync(TRegion region, string key, object value, TimeSpan expiration);
    }
}
