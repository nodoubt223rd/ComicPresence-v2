using System.Collections.Generic;

namespace ComicPresence.Common.Collections
{
    public static class ListExtensions
    {
        public static HashSet<T> ToHashSet<T>(this IList<T> list)
        {
            return new HashSet<T>(list);
        }
    }
}
