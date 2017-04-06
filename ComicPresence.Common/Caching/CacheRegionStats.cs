using System.Threading;

namespace ComicPresence.Common.Caching
{
    public class CacheRegionStats<TRegion>
    {
        public TRegion Region { get; protected set; }

        public int Hits { get { return _hits; } set { _hits = value; } }
        private int _hits;

        public int Misses { get { return _misses; } set { _misses = value; } }
        private int _misses;

        public CacheRegionStats()
        {
        }

        public CacheRegionStats(TRegion region)
        {
            Region = region;
        }

        public void IncrementHits()
        {
            Interlocked.Increment(ref _hits);
        }

        public void IncrementMisses()
        {
            Interlocked.Increment(ref _misses);
        }
    }
}
