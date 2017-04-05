using System;

namespace ComicPresence.Common.Patterns
{
    public abstract class Singleton<T> where T : class
    {
        private static readonly Lazy<T> _instance = new Lazy<T>(() => CreateInstance(),
            System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

        public static T Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public static T CreateInstance()
        {
            return Activator.CreateInstance(typeof(T), true) as T;
        }
    }
}
