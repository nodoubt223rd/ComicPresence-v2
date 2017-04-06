
namespace ComicPresence.Common.Caching
{
    internal class CacheResult<TResult>
    {
        public TResult Value { get; set; }
        public bool Found { get; set; }
    }
}
