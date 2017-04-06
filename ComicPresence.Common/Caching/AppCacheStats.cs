using System.Collections.Generic;

namespace ComicPresence.Common.Caching
{
    public class AppCacheStats
    {
        public Common.Config.ApplicationId ApplicationId { get; set; }
        /// <summary>
        /// Keys are ComicPresence.Common.Config.ApplicationIds (ints). Types as strings for serialization. 
        /// </summary>
        public IDictionary<string, CacheRegionStats<CacheRegion>> InstanceCacheStats { get; set; }
        public IDictionary<string, CacheRegionStats<CacheRegion>> LocalInstanceCacheStats { get; set; }
    }
}
