using System;

namespace LSCS.Api.Exceptions
{
    public class InvalidQueryException : LscsApiException
    {
        public InvalidQueryException() { }

        public InvalidQueryException(string message) : base(message) { }

        public InvalidQueryException(string message, Exception innerException) : base(message, innerException) { }
    }
}