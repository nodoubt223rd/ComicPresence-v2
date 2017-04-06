
namespace ComicPresence.Common.Caching
{
    /// <summary>
    /// Wrapper for cache values
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CacheItem<T>
    {
        public T Value { get; set; }

        /// <summary>
        /// Used to invalidate cached data that is based on oudated models
        /// </summary>
        public string ItemVersion { get; set; }
    }
}
