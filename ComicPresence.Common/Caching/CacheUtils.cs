using System;
using System.Linq;

using ComicPresence.Common.Text;

namespace ComicPresence.Common.Caching
{
    public class CacheUtils
    {
        /// <summary>
        /// Creates a single string key that can be used as a cache key. Nulls and empty strings are equivalent. 
        /// </summary>
        /// <param name="keyParams"></param>
        /// <returns></returns>
        public static string CreateKey(params object[] keyParams)
        {
            return StringListEncoder.Encode(keyParams.Select(param => param == null ? String.Empty : param.ToString()));
        }
    }
}
