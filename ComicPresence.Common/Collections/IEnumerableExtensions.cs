using System;
using System.Collections.Generic;

namespace ComicPresence.Common.Collections
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> ienumerable, Action<T> action)
        {
            foreach (T item in ienumerable)
            {
                action(item);
            }
        }
    }
}
