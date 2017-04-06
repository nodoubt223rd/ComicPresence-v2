using System;

namespace ComicPresence.Common.Caching
{
    public static class CacheSettings
    {
        public static readonly TimeSpan cDefaultSitecoreExpiration = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Default expiration for most non-user data
        /// </summary>
        public static readonly TimeSpan cDefaultNonUserExpiration = TimeSpan.FromHours(6);

        /// <summary>
        /// Default expiration for data the user can change
        /// </summary>
        public static readonly TimeSpan cDefaultUserExpiration = TimeSpan.FromMinutes(15);

        /// <summary>
        /// Rates, fees, countries, etc. 
        /// </summary>
        public static readonly TimeSpan cPricingDataExpiration = TimeSpan.FromMinutes(2);
    }
}
