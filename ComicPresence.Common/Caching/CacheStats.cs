using System.Collections.Concurrent;

namespace ComicPresence.Common.Caching
{
    public class CacheStats<TRegion>
    {
        public ConcurrentDictionary<TRegion, CacheRegionStats<TRegion>> CacheRegionStatsDict { get; }

        public CacheStats()
        {
            CacheRegionStatsDict = new ConcurrentDictionary<TRegion, CacheRegionStats<TRegion>>();
        }

        public void IncrementHits(TRegion region)
        {
            CacheRegionStats<TRegion> stats = CacheRegionStatsDict.GetOrAdd(region, (aRegion) => new CacheRegionStats<TRegion>(aRegion));
            stats.IncrementHits();
        }

        public void IncrementMisses(TRegion region)
        {
            CacheRegionStats<TRegion> stats = CacheRegionStatsDict.GetOrAdd(region, (aRegion) => new CacheRegionStats<TRegion>(aRegion));
            stats.IncrementMisses();
        }
    }
}
