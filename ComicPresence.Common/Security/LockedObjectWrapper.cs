

namespace ComicPresence.Common.Security
{
    public class LockedObjectWrapper<T>
    {
        public T Object { get; set; }
        public string Random { get; set; }
    }
}
