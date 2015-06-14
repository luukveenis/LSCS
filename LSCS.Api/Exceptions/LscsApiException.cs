using System;

namespace LSCS.Api.Exceptions
{
    public abstract class LscsApiException : Exception
    {
        protected LscsApiException() { }

        protected LscsApiException(string message) : base(message) { }

        protected LscsApiException(string message, Exception innerException) : base(message, innerException) { }
    }
}