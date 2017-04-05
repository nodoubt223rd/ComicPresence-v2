using System;

namespace ComicPresence.Common.Data
{
    public class DataException : Exception
    {
        public DataException()
        {

        }

        public DataException(string message) : base(message)
        {

        }
    }
}
