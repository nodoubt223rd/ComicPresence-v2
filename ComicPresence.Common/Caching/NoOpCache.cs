using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComicPresence.Common.Caching
{
    /// <summary>
    /// Does not do any caching. Returns default values, etc. 
    /// </summary>
    /// <typeparam name="TRegion"></typeparam>
    internal class NoOpCache<TRegion> : ICache<TRegion>
    {
        public CacheStats<TRegion> CacheStats
        {
            get
            {
                CacheStats<TRegion> stats = new CacheStats<TRegion>();
                return stats;
            }
        }

        public TResult Get<TResult>(TRegion region, string key)
        {
            return default(TResult);
        }

        public TResult Get<TResult>(TRegion region, string key, TimeSpan expiration, Func<TResult> dataRetrievalFunc)
        {
            return dataRetrievalFunc();
        }

        public async Task<TResult> GetAsync<TResult>(TRegion region, string key)
        {
            return await Task.FromResult<TResult>(default(TResult)).ConfigureAwait(false);
        }

        public async Task<TResult> GetAsync<TResult>(TRegion region, string key, TimeSpan expiration, Func<Task<TResult>> dataRetrievalFunc)
        {
            return await dataRetrievalFunc().ConfigureAwait(false);
        }

        public async Task GetParallelAsync(params CacheGetCommand<object, TRegion>[] commands)
        {
            List<Tuple<Task<object>, CacheGetCommand<object, TRegion>>> tasksAndCommands = new List<Tuple<Task<object>, CacheGetCommand<object, TRegion>>>();
            foreach (CacheGetCommand<object, TRegion> cmd in commands)
            {
                tasksAndCommands.Add(new Tuple<Task<object>, CacheGetCommand<object, TRegion>>(cmd.DataRetrievalFunc(), cmd));
            }

            foreach (Tuple<Task<object>, CacheGetCommand<object, TRegion>> taskAndCommand in tasksAndCommands)
            {
                taskAndCommand.Item2.Result = await taskAndCommand.Item1.ConfigureAwait(false);
            }
        }

        public void Remove(TRegion region, string key)
        {

        }

        public void RemoveAll()
        {

        }

        public Task RemoveAsync(TRegion region, string key)
        {
            return Task.CompletedTask;
        }

        public void Put(TRegion region, string key, object value, TimeSpan expiration)
        {

        }

        public Task PutAsync(TRegion region, string key, object value, TimeSpan expiration)
        {
            return Task.CompletedTask;
        }
    }
}
