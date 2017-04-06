
namespace ComicPresence.Common.Caching
{
    public abstract class CacheCommand<TRegion>
    {
        public TRegion Region { get; protected set; }
        public string Key { get; protected set; }

        /// <summary>
        /// Can be used by user code to track arbitrary state
        /// </summary>
        public object UserState { get; set; }

        public CacheCommand(TRegion region, string key)
        {
            Region = region;
            Key = key;
        }
    }
}
