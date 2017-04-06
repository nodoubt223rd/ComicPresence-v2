using System;
using System.Threading.Tasks;

namespace ComicPresence.Common.Caching
{
    public class CacheGetCommand<TResult, TRegion> : CacheCommand<TRegion> where TResult : class
    {
        public TimeSpan Expiration { get; protected set; }
        public Func<Task<TResult>> DataRetrievalFunc { get; protected set; }
        public TResult Result { get; internal set; }
        public Type ResultType { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key"></param>
        /// <param name="expiration"></param>
        /// <param name="dataRetrievalFunc">This should be async</param>
        /// <param name="resultType"></param>
        public CacheGetCommand(TRegion region, string key, TimeSpan expiration, Func<Task<TResult>> dataRetrievalFunc, Type resultType)
            : base(region, key)
        {
            if (dataRetrievalFunc == null)
                throw new ArgumentNullException("dataRetrievalFunc");
            if (resultType == null)
                throw new ArgumentNullException("resultType");

            Expiration = expiration;
            DataRetrievalFunc = dataRetrievalFunc;
            ResultType = resultType;
        }
    }

    public class CacheGetCommand<TRegion> : CacheGetCommand<object, TRegion>
    {
        public CacheGetCommand(TRegion region, string key, TimeSpan expiration, Func<Task<object>> dataRetrievalFunc, Type resultType)
            : base(region, key, expiration, dataRetrievalFunc, resultType)
        {
        }
    }

    public class CacheGetCommand : CacheGetCommand<CacheRegion>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="region"></param>
        /// <param name="key">Can be null</param>
        /// <param name="expiration"></param>
        /// <param name="dataRetrievalFunc"></param>
        /// <param name="resultType"></param>
        public CacheGetCommand(CacheRegion region, string key, TimeSpan expiration, Func<Task<object>> dataRetrievalFunc, Type resultType)
            : base(region, key, expiration, dataRetrievalFunc, resultType)
        {
        }
    }
}
