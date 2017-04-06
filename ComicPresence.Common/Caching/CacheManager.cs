using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicPresence.Common.Caching
{
    /// <summary>
    /// Thread-safe. Singleton/Doubleton. 
    /// </summary>
    public class CacheManager : ICache<CacheRegion>
    {
        private static readonly Lazy<CacheManager> _instance = new Lazy<CacheManager>(() => new CacheManager(),
            System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        private static readonly Lazy<CacheManager> _localInstance = new Lazy<CacheManager>(() => new CacheManager(),
            System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

        private ICache<CacheRegion> Cache { get; set; }

        public CacheStats<CacheRegion> CacheStats
        {
            get
            {
                CheckInitialized();
                return Cache.CacheStats;
            }
        }

        /// <summary>
        /// Main instance
        /// </summary>
        public static CacheManager Instance
        {
            get { return _instance.Value; }
        }

        /// <summary>
        /// Local (not remote) cache instance. Generally used if the main Instance is a distributed/remote cache. 
        /// </summary>
        public static CacheManager LocalInstance
        {
            get { return _localInstance.Value; }
        }

        private CacheManager()
        {
        }

        public void InitializeMemoryCache()
        {
            CheckAlreadyInitialized();
            Cache = new MemoryCache<CacheRegion>();
        }

        public void InitializeNoOpCache()
        {
            CheckAlreadyInitialized();
            Cache = new NoOpCache<CacheRegion>();
        }

        public TResult Get<TResult>(CacheRegion region, string key)
        {
            CheckInitialized();
            return Cache.Get<TResult>(region, key);
        }

        public TResult Get<TResult>(CacheRegion region, string key, TimeSpan expiration, Func<TResult> dataRetrievalFunc)
        {
            CheckInitialized();
            return Cache.Get(region, key, expiration, dataRetrievalFunc);
        }

        public TResult Get<TResult>(CacheGetCommand command)
        {
            CheckInitialized();
            return Cache.Get(command.Region, command.Key, command.Expiration, delegate () {
                Task<object> task = command.DataRetrievalFunc();
                return (TResult)task.Result;
            });
        }

        public async Task<TResult> GetAsync<TResult>(CacheRegion region, string key)
        {
            CheckInitialized();
            return await Cache.GetAsync<TResult>(region, key).ConfigureAwait(false);
        }

        public async Task<TResult> GetAsync<TResult>(CacheRegion region, string key, TimeSpan expiration, Func<Task<TResult>> dataRetrievalFunc)
        {
            CheckInitialized();
            return await Cache.GetAsync(region, key, expiration, dataRetrievalFunc).ConfigureAwait(false);
        }

        public async Task<TResult> GetAsync<TResult>(CacheGetCommand command)
        {
            CheckInitialized();
            return await Cache.GetAsync(command.Region, command.Key, command.Expiration, async delegate () {
                Task<object> task = command.DataRetrievalFunc();
                return (TResult)(await task.ConfigureAwait(false));
            }).ConfigureAwait(false);
        }

        public async Task GetParallelAsync(params CacheGetCommand<object, CacheRegion>[] commands)
        {
            CheckInitialized();
            await Cache.GetParallelAsync(commands).ConfigureAwait(false);
        }

        public async Task GetParallelAsync(IEnumerable<CacheGetCommand<object, CacheRegion>> commands)
        {
            CheckInitialized();
            await Cache.GetParallelAsync(commands.ToArray()).ConfigureAwait(false);
        }

        /// <summary>
        /// Results are added to the command objects
        /// </summary>
        /// <param name="commands"></param>
        /// <returns></returns>
        public void GetParallel(params CacheGetCommand<object, CacheRegion>[] commands)
        {
            CheckInitialized();
            Cache.GetParallelAsync(commands).Wait();
        }

        public void GetParallel(IEnumerable<CacheGetCommand<object, CacheRegion>> commands)
        {
            CheckInitialized();
            Cache.GetParallelAsync(commands.ToArray()).Wait();
        }

        public void Remove(CacheRegion region, string key)
        {
            CheckInitialized();
            Cache.Remove(region, key);
        }

        public void RemoveAll()
        {
            CheckInitialized();
            Cache.RemoveAll();
        }

        public async Task RemoveAsync(CacheRegion region, string key)
        {
            CheckInitialized();
            await Cache.RemoveAsync(region, key).ConfigureAwait(false);
        }

        public void Put(CacheRegion region, string key, object value, TimeSpan expiration)
        {
            CheckInitialized();
            Cache.Put(region, key, value, expiration);
        }

        public async Task PutAsync(CacheRegion region, string key, object value, TimeSpan expiration)
        {
            CheckInitialized();
            await Cache.PutAsync(region, key, value, expiration).ConfigureAwait(false);
        }

        private void CheckInitialized()
        {
            if (Cache == null)
                throw new InvalidOperationException("Not initialized");
        }

        private void CheckAlreadyInitialized()
        {
            if (Cache != null)
                throw new InvalidOperationException("Already initialized");
        }
    }
}
