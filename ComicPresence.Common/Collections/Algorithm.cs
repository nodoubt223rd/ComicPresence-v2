using System;
using System.Collections.Generic;

namespace ComicPresence.Common.Collections
{
    public static class Algorithm
    {
        /// <summary>
        /// If first and second do not have the same number of elements, action is invoked for as many pairs as exist, but not for any unpaired elements
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="action"></param>
        public static void ForEachPair<TFirst, TSecond>(IEnumerable<TFirst> first, IEnumerable<TSecond> second, Action<TFirst, TSecond> action)
        {
            if (first == null)
                throw new ArgumentNullException("first");
            if (second == null)
                throw new ArgumentNullException("second");
            if (action == null)
                throw new ArgumentNullException("action");

            IEnumerator<TFirst> firstItr = first.GetEnumerator();
            IEnumerator<TSecond> secondItr = second.GetEnumerator();

            while (firstItr.MoveNext() && secondItr.MoveNext())
            {
                action(firstItr.Current, secondItr.Current);
            }
        }
    }
}
