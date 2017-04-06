using System;
using System.Runtime.Serialization;

namespace ComicPresence.Common.Security.Validation
{
    public class ValidationException : ApplicationException
    {
        public ValidationException() { }

        public ValidationException(string message)
            : base(message) { }

        public ValidationException(string message, Exception innerException)
            : base(message, innerException) { }

        public ValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
