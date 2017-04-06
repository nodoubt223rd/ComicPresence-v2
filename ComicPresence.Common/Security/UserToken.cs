using System;

namespace ComicPresence.Common.Security
{
    public class UserToken
    {
        public string Id { get; set; }
        public DateTime DateTimeUtc { get; set; }
        public string Type { get; set; }
    }
}
