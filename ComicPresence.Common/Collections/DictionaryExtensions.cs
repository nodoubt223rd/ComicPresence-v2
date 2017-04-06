using System.Collections.Generic;

namespace ComicPresence.Common.Collections
{
    public static class DictionaryExtensions
    {
        public static TValue ItemOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            else
            {
                return default(TValue);
            }
        }
    }
}
